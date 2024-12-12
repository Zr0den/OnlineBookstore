using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> CreateAsync(OrderItem orderItem);
        Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task DeleteAsync(int id);
    }
}
