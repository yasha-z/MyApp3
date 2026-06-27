namespace Session1
{
    // TASK 3.3 — Service contract uses DTOs only; no Book entity leaks to controllers.
    public interface IBookService
    {
        // optional author name filter + pagination
        Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize);

        Task<BookResponseDTO> GetByIdAsync(int id);

        Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);

        Task UpdateAsync(int id, BookUpdateDTO dto);

        Task DeleteAsync(int id);
    }
}
