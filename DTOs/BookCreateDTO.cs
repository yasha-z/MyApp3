using System.ComponentModel.DataAnnotations;
//DTOs are often called data containers
namespace Session1
{
    // TASK 3.1 — DTO for POST /api/books request body.

    // ASP.NET Core automatically returns 400 Bad Request with validation errors when
    // ModelState is invalid (e.g. empty Title → 400 without extra controller code)

    //controller ke paas jaiga hee nahin if validation fails
    public class BookCreateDTO
    {
        // Title is required and cannot be blank — [Required] rejects null; [MinLength(1)] rejects "".
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        public string Title { get; set; } = string.Empty;

        // Reasonable publication-year bounds for demo data validation.
        [Range(1000, 2100, ErrorMessage = "Year must be between 1000 and 2100.")]
        public int Year { get; set; }

        // Page count must be at least 1 for a real book.
        [Range(1, int.MaxValue, ErrorMessage = "PageCount must be at least 1.")]
        public int PageCount { get; set; }

        // Foreign key to Author — must reference an existing author id in the store.
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a positive integer.")]
        public int AuthorId { get; set; }
    }
}
