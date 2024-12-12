using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IInventoryRepository
    {
        Task<int?> GetStockLevelAsync(string isbn);
        Task SetStockLevelAsync(string isbn, int stockLevel);
        Task UpdateStockLevelAsync(string isbn, int quantity);
    }
}
