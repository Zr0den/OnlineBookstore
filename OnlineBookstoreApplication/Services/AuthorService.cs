using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using OnlineBookstoreInfrastructure.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IDatabase _redisDatabase;
        private const int CacheTtl = 24; //Hours

        public AuthorService(IAuthorRepository authorRepository, IConnectionMultiplexer redis)
        {
            _authorRepository = authorRepository;
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            if (string.IsNullOrEmpty(author.Name))
                throw new ArgumentException("Author name is required.");

            return await _authorRepository.CreateAuthorAsync(author);
        }

        public async Task<Author> GetAuthorByIdAsync(string id)
        {
            var cacheKey = $"Author:{id}";

            // Exists in cache? use cache, otherwise use repository and then save in cache for later
            var cachedAuthor = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedAuthor.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Author>(cachedAuthor);
            }

            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author != null)
            {
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(author), TimeSpan.FromHours(CacheTtl));
            }
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            var success = await _authorRepository.UpdateAuthorAsync(author);

            if (success)
            {
                var cacheKey = $"Book:{author.Id}";

                //No need to check for existing value - Redis will create new kvp if not exists, and just overwrite if exists.
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(author), TimeSpan.FromHours(CacheTtl));
            }

            return success;
        }

        public async Task<bool> DeleteAuthorAsync(string id)
        {
            var cacheKey = $"Author:{id}";

            //Don't technically need to check if the key exists, it just *feels* better
            if (_redisDatabase.KeyExists(cacheKey))
            {
                await _redisDatabase.KeyDeleteAsync(cacheKey);
            }

            return await _authorRepository.DeleteAuthorAsync(id);
        }
    }
}
