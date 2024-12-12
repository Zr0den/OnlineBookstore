using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> CreateAuthorAsync(Author author);
        Task<Author> GetAuthorByIdAsync(string id);
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<bool> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(string id);
    }
}
