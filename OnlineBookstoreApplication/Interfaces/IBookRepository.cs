using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateBookAsync(Book book);
    }
}
