using MongoDB.Driver;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class MongoDbBookRepository : IBookService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public MongoDbBookRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _booksCollection = database.GetCollection<Book>("Books");
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            await _booksCollection.InsertOneAsync(book);
            return book;
        }
    }
}
