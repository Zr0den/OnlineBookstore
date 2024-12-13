using Microsoft.EntityFrameworkCore;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public OrderRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
