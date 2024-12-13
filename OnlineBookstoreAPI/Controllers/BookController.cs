using Microsoft.AspNetCore.Mvc;
using OnlineBookstoreCore.Dto;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;

namespace OnlineBookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;

        public BookController(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBookByIsbn(string isbn)
        {
            var book = await _bookService.GetBookByIsbnAsync(isbn);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("authors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            var createdBook = await _bookService.CreateBookAsync(book);
            return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = createdBook.ISBN }, createdBook);
        }
    }
}
