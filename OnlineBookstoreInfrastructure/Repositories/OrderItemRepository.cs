using Microsoft.EntityFrameworkCore;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public OrderItemRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems)
        {
            _dbContext.OrderItems.AddRange(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _dbContext.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
    }
}
