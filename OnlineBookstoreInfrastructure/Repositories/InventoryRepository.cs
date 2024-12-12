using OnlineBookstoreCore.Interfaces;
using StackExchange.Redis;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDatabase _redisDatabase;

        public InventoryRepository(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task<int?> GetStockLevelAsync(string isbn)
        {
            var stock = await _redisDatabase.StringGetAsync($"BookInventory:{isbn}");
            return stock.HasValue ? (int?)int.Parse(stock) : null;
        }

        public async Task SetStockLevelAsync(string isbn, int stockLevel)
        {
            await _redisDatabase.StringSetAsync($"BookInventory:{isbn}", stockLevel);
        }

        public async Task<bool> UpdateStockLevelAsync(string isbn, int quantity)
        {
            if (quantity == 0)
            {
                //Not sure why you would call this with 0, but it is the same as doing nothing so here we are
                return false;
            }

            var result = quantity > 0 ? await _redisDatabase.StringIncrementAsync($"BookInventory:{isbn}", quantity) :
                           await _redisDatabase.StringDecrementAsync($"BookInventory:{isbn}", -quantity);
            return result > 0;
        }
    }
}
