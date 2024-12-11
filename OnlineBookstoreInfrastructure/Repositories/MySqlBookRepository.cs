using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreInfrastructure.Repositories
{
    public class MySqlBookRepository : IBookService
    {
        private readonly BookstoreDbContext _dbContext;

        public MySqlBookRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            _dbContext.MySqlBooks.Add(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }
    }
}
