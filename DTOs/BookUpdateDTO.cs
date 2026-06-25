using System.ComponentModel.DataAnnotations;

namespace Session1
{
    // TASK 3.1 — DTO for PUT /api/books/{id} request body.
    // Same validation rules as create; the route supplies the book id, not the body.
    public class BookUpdateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Year must be between 1000 and 2100.")]
        public int Year { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageCount must be at least 1.")]
        public int PageCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a positive integer.")]
        public int AuthorId { get; set; }
    }
}
