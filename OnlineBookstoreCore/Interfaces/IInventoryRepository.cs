using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IInventoryRepository
    {
        Task<int?> GetStockLevelAsync(string bookId);
        Task SetStockLevelAsync(string bookId, int stockLevel);
        Task DecreaseStockLevelAsync(string bookId, int quantity);
        Task IncreaseStockLevelAsync(string bookId, int quantity);
    }
}
