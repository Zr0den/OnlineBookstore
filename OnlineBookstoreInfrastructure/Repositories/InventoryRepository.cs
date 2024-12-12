using OnlineBookstoreCore.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDatabase _redisDatabase;

        public InventoryRepository(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task<int?> GetStockLevelAsync(string bookId)
        {
            var stock = await _redisDatabase.StringGetAsync($"BookInventory:{bookId}");
            return stock.HasValue ? (int?)int.Parse(stock) : null;
        }

        public async Task SetStockLevelAsync(string bookId, int stockLevel)
        {
            await _redisDatabase.StringSetAsync($"BookInventory:{bookId}", stockLevel);
        }

        public async Task DecreaseStockLevelAsync(string bookId, int quantity)
        {
            await _redisDatabase.StringDecrementAsync($"BookInventory:{bookId}", quantity);
        }

        public async Task IncreaseStockLevelAsync(string bookId, int quantity)
        {
            await _redisDatabase.StringIncrementAsync($"BookInventory:{bookId}", quantity);
        }
    }
}
