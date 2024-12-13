using MongoDB.Bson.IO;
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
using Order = OnlineBookstoreCore.Models.Order;

namespace OnlineBookstoreApplication.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDatabase _redisDatabase;
        private const int CacheTtl = 10; // minutes

        public OrderService(IOrderRepository orderRepository, IConnectionMultiplexer redisConnection)
        {
            _orderRepository = orderRepository;
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new ArgumentException("Order must have at least one item.");
            }

            Order createdOrder = await _orderRepository.CreateAsync(order);

            //Cache newly created orders for 10 minutes, 
            string cacheKey = $"Order:{createdOrder.Id}";
            await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(createdOrder), TimeSpan.FromHours(CacheTtl));

            await _redisDatabase.ListLeftPushAsync("RecentOrders", cacheKey);
            //Only most recent 100 orders (arbitrary number)
            await _redisDatabase.ListTrimAsync("RecentOrders", 0, 99);

            return createdOrder;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            string cacheKey = $"Order:{id}";

            var cachedOrder = await _redisDatabase.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedOrder))
            {
                // Deserialize the cached order
                return JsonSerializer.Deserialize<Order>(cachedOrder);
            }

            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            var success = await _orderRepository.UpdateAsync(order);

            if (success)
            {
                var cacheKey = $"Order:{order.Id}";

                if (_redisDatabase.KeyExists(cacheKey))
                {
                    await _redisDatabase.KeyDeleteAsync(cacheKey);
                }
            }

            return success;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var cacheKey = $"Order:{id}";

            //Don't technically need to check if the key exists, it just *feels* better
            if (_redisDatabase.KeyExists(cacheKey))
            {
                await _redisDatabase.KeyDeleteAsync(cacheKey);
            }

            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync()
        {
            // Retrieve up to 100 recent order keys from the Redis list
            var recentOrderKeys = await _redisDatabase.ListRangeAsync("RecentOrders", 0, 99);

            var recentOrders = new List<Order>();

            foreach (var key in recentOrderKeys)
            {
                // Convert the RedisValue (key) to a string before using it
                var cachedOrder = await _redisDatabase.StringGetAsync((string)key);

                if (!string.IsNullOrEmpty(cachedOrder))
                {
                    recentOrders.Add(JsonSerializer.Deserialize<Order>(cachedOrder));
                }
            }

            return recentOrders;
        }
    }
}
