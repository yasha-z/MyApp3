using Microsoft.AspNetCore.Mvc;

namespace Session1
{
    // TASK 3.6 — Full CRUD via IBookService + DTOs only; persistence stays in the service layer.
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        // TASK 3.5 — Service injected by DI; never use 'new BookService()' here.
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET /api/books?author=Rowling&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<List<BookResponseDTO>>> GetAll(
            [FromQuery] string? author,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var books = await _bookService.GetAllAsync(author, page, pageSize);
            return Ok(books);
        }

        // GET /api/books/{id} — missing id → 404 ProblemDetails via BookNotFoundExceptionHandler
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookResponseDTO>> GetById(int id)
        {//actionresult returns either a BookResponseDTO or a 404 Not Found response if the book is not found
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }

        // POST /api/books invalid body (e.g. empty title) → automatic 400 from [ApiController] + validation
        [HttpPost]
        public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookCreateDTO dto)
        {
            var created = await _bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            // define one to many relationship between author and book, so that when we create a book we can also specify the author of the book
        }

        // PUT /api/books/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO dto)
        {
            await _bookService.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/books/{id} — success returns 204 No Content
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
