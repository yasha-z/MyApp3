namespace Session1
{
    // TASK 3.1 — DTO returned to API clients (GET, POST 201, etc.).
    // Record with init-only properties: immutable shape for responses, no validation needed on output.
    public record BookResponseDTO
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int Year { get; init; }
        public int PageCount { get; init; }
        public int AuthorId { get; init; }

        // Denormalized author name so clients don't need a second request for display.
        public string AuthorName { get; init; } = string.Empty;
    }
}
