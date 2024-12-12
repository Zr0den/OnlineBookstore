using MongoDB.Driver;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IMongoCollection<Author> _authorsCollection;

        public AuthorRepository(BookstoreDbContext dbContext)
        {
            _authorsCollection = dbContext.Authors;
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            await _authorsCollection.InsertOneAsync(author);
            return author;
        }

        public async Task<Author> GetAuthorByIdAsync(string id)
        {
            return await _authorsCollection
                .Find(author => author.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _authorsCollection
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            var result = await _authorsCollection.ReplaceOneAsync(
                a => a.Id == author.Id,
                author
            );
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAuthorAsync(string id)
        {
            var result = await _authorsCollection.DeleteOneAsync(author => author.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
