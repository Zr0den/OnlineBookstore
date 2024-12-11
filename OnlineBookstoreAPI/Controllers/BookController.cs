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

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto createBookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = createBookDto.Title,
                AuthorId = createBookDto.AuthorId,
                ISBN = createBookDto.ISBN,
                Price = createBookDto.Price
            };

            var createdBook = await _bookService.CreateBookAsync(book);
            return CreatedAtAction(nameof(CreateBook), new { id = createdBook.Id }, createdBook);
        }
    }
}
