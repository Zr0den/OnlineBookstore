using OnlineBookstoreCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<int?> GetStockLevelAsync(string isbn)
        {
            return await _inventoryRepository.GetStockLevelAsync(isbn);
        }

        public async Task SetStockLevelAsync(string isbn, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Stock level cannot be negative.");

            await _inventoryRepository.SetStockLevelAsync(isbn, quantity);
        }

        public async Task<bool> UpdateStockAsync(string isbn, int quantityChange)
        {
            // Delegate the call to the repository
            var stockUpdated = await _inventoryRepository.UpdateStockLevelAsync(isbn, quantityChange);

            // Return the result indicating whether the stock was updated successfully
            return stockUpdated;
        }
    }
}
