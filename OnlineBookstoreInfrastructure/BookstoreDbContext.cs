using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OnlineBookstoreCore.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreInfrastructure
{
    public class BookstoreDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OnlineBookstoreCore.Models.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Book> MySqlBooks { get; set; }

        // MongoDB Client
        private readonly IMongoDatabase _mongoDatabase;
        public IMongoCollection<Book> MongoDbBooks => _mongoDatabase.GetCollection<Book>("Books");
        public IMongoCollection<Author> Authors => _mongoDatabase.GetCollection<Author>("Authors");

        // Redis Connection
        private readonly IConnectionMultiplexer _redisConnection;
        public IDatabase RedisDatabase => _redisConnection.GetDatabase();

        public BookstoreDbContext(
            DbContextOptions<BookstoreDbContext> options,
            IMongoClient mongoClient, // Inject MongoClient for MongoDB
            IConnectionMultiplexer redisConnection) // Inject RedisConnection for Redis
        {
            // Initialize MySQL DbContext
            var mysqlContext = new DbContext(options);
            Customers = mysqlContext.Set<Customer>();
            Orders = mysqlContext.Set<OnlineBookstoreCore.Models.Order>();
            OrderItems = mysqlContext.Set<OrderItem>();

            // Initialize MongoDB connection
            _mongoDatabase = mongoClient.GetDatabase("BookstoreDB");

            // Initialize Redis connection
            _redisConnection = redisConnection;
        }

        // Custom Methods for Redis (e.g., for inventory)
        public void UpdateInventory(string bookId, int quantityChange)
        {
            // Redis key for book inventory
            var inventoryKey = $"inventory:{bookId}";

            // Update the stock using Redis' INCR/DECR commands for atomic operations
            if (quantityChange > 0)
                RedisDatabase.StringIncrement(inventoryKey, quantityChange);
            else
                RedisDatabase.StringDecrement(inventoryKey, -quantityChange);
        }

        // Custom Methods for MongoDB (e.g., finding a book by ISBN)
        public Book GetBookByIsbn(string isbn)
        {
            return MongoDbBooks.Find(b => b.ISBN == isbn).FirstOrDefault();
        }
    }
}
