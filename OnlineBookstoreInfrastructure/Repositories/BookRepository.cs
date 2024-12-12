using MongoDB.Driver;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BookRepository(BookstoreDbContext dbContext)
        {
            _booksCollection = dbContext.Books;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            await _booksCollection.InsertOneAsync(book);
            return book;
        }

        public async Task<Book> GetBookByIsbnAsync(string isbn)
        {
            return await _booksCollection
                .Find(book => book.ISBN == isbn)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _booksCollection
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _booksCollection
                .Find(book => book.Author.Name == author)
                .ToListAsync();
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            var result = await _booksCollection.ReplaceOneAsync(
                b => b.ISBN == book.ISBN,
                book
            );
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteBookAsync(string isbn)
        {
            var result = await _booksCollection.DeleteOneAsync(
                book => book.ISBN == isbn
            );
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
