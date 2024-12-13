using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetRecentOrdersAsync();
    }
}
