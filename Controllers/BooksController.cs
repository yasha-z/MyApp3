using Microsoft.AspNetCore.Mvc;
//a controller takes the request, hands it to the service, returns the result (from session 3 slides)
namespace Session1
{
    // TASK 3.6 — Full CRUD via IBookService + DTOs only; persistence stays in the service layer.
    [ApiController] // this attribute tells that this class is a controller and it will handle the request and return the response
    // from slides: it does auto-valiation (reads [Required], [Range], [StringLength] on your DTOs)
    // and binding source inference ( figures out where data comes from automatically)


//lvl 1 class -> which controller?
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        // TASK 3.5 — Service injected by DI; never use 'new BookService()' here
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }


//lvl 2 method -> which action?
        // GET /api/books?author=Rowling&page=1&pageSize=10
        [HttpGet]

        //action results: ok, notfound, badrequest, created, no content

        //CONTROLLER ↔ DTOs only (never sees the entity)
        // dtos: Only safe, needed fields
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
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }

        // POST /api/books = invalid body (e.g. empty title) → automatic 400 from [ApiController] + validation
        [HttpPost]
        public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookCreateDTO dto)//waise toh automatically [FromBody] hojati hai
         //but we can explicitly mention it for clarity
        {
            var created = await _bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/books/{id}
        [HttpPut("{id:int}")]

        //iaction result: when nothing to return
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
