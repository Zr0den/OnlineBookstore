using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IInventoryService
    {
        Task<int?> GetStockLevelAsync(string isbn);
        Task SetStockLevelAsync(string isbn, int quantity);
        Task UpdateStockAsync(string isbn, int quantityChange);
    }
}
