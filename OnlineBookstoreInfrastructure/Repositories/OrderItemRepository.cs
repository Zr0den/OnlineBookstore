using Microsoft.EntityFrameworkCore;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public OrderItemRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();
            return orderItem;
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

        public async Task DeleteAsync(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _dbContext.OrderItems.Remove(orderItem);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
