﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OnlineBookstoreCore.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = OnlineBookstoreCore.Models.Order;

namespace OnlineBookstoreInfrastructure
{
    public class BookstoreDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // MongoDB Client
        private readonly IMongoDatabase _mongoDatabase;
        public IMongoCollection<Book> Books => _mongoDatabase.GetCollection<Book>("Books");
        public IMongoCollection<Author> Authors => _mongoDatabase.GetCollection<Author>("Authors");

        // Redis Connection
        private readonly IConnectionMultiplexer _redisConnection;
        public IDatabase RedisDatabase => _redisConnection.GetDatabase();

        public BookstoreDbContext(
            DbContextOptions<BookstoreDbContext> options,
            IMongoClient mongoClient,
            IConnectionMultiplexer redisConnection)
        {
            // Initialize MySQL DbContext
            var mysqlContext = new DbContext(options);
            Customers = mysqlContext.Set<Customer>();
            Orders = mysqlContext.Set<Order>();
            OrderItems = mysqlContext.Set<OrderItem>();

            // Initialize MongoDB connection
            _mongoDatabase = mongoClient.GetDatabase("BookstoreDB");

            // Initialize Redis connection
            _redisConnection = redisConnection;
        }
    }
}
