using OnlineBookstoreCore.Models;
using OnlineBookstoreCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public CustomerRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _dbContext.Customers.FindAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}