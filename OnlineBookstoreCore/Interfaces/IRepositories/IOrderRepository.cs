using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<Order> GetByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int orderId);
    }
}
