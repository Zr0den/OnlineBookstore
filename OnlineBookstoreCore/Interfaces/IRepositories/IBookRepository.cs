using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateBookAsync(Book book);
        Task<Book> GetBookByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(string isbn);
    }
}
