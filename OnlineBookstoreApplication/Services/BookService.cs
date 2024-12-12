using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            if (string.IsNullOrEmpty(book.ISBN))
                throw new ArgumentException("ISBN is required to create a book.");

            return await _bookRepository.CreateBookAsync(book);
        }

        public async Task<Book> GetBookByIsbnAsync(string isbn)
        {
            return await _bookRepository.GetBookByIsbnAsync(isbn);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            return await _bookRepository.UpdateBookAsync(book);
        }

        public async Task<bool> DeleteBookAsync(string isbn)
        {
            return await _bookRepository.DeleteBookAsync(isbn);
        }
    }
}
