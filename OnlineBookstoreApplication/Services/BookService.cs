using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IDatabase _redisDatabase;
        private const int CacheTtl = 24; //Hours

        public BookService(IBookRepository bookRepository, IConnectionMultiplexer redis)
        {
            _bookRepository = bookRepository;
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            if (string.IsNullOrEmpty(book.ISBN))
                throw new ArgumentException("ISBN is required to create a book.");

            return await _bookRepository.CreateBookAsync(book);
        }

        public async Task<Book> GetBookByIsbnAsync(string isbn)
        {
            var cacheKey = $"Book:{isbn}";

            // Exists in cache? use cache, otherwise use repository and then save in cache for later
            var cachedBook = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedBook.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Book>(cachedBook);
            }

            var book = await _bookRepository.GetBookByIsbnAsync(isbn);
            if (book != null)
            {
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(book), TimeSpan.FromHours(CacheTtl));
            }
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            var success = await _bookRepository.UpdateBookAsync(book);

            if (success)
            {
                var cacheKey = $"Book:{book.ISBN}";

                //No need to check for existing value - Redis will create new kvp if not exists, and just overwrite if exists.
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(book), TimeSpan.FromHours(CacheTtl));
            }

            return success;
        }

        public async Task<bool> DeleteBookAsync(string isbn)
        {
            var cacheKey = $"Book:{isbn}";

            //Don't technically need to check if the key exists, it just *feels* better
            if (_redisDatabase.KeyExists(cacheKey))
            {
                await _redisDatabase.KeyDeleteAsync(cacheKey);
            }

            return await _bookRepository.DeleteBookAsync(isbn);
        }
    }
}
