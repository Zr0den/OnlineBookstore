using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            if (string.IsNullOrEmpty(author.Name))
                throw new ArgumentException("Author name is required.");

            return await _authorRepository.CreateAuthorAsync(author);
        }

        public async Task<Author> GetAuthorByIdAsync(string id)
        {
            return await _authorRepository.GetAuthorByIdAsync(id);
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            return await _authorRepository.UpdateAuthorAsync(author);
        }

        public async Task<bool> DeleteAuthorAsync(string id)
        {
            return await _authorRepository.DeleteAuthorAsync(id);
        }
    }
}
