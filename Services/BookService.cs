using System.Linq;

namespace Session1
{
    // TASK 3.4 — Service uses BookMapper and works with DTOs; entity access stays here.
    public class BookService : IBookService
    {
        public Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
        {
            // Start from full in-memory list, then apply optional author filter.
            IEnumerable<Book> query = InMemoryStore.Books;

            if (!string.IsNullOrWhiteSpace(author))
            {
                // Case-insensitive match on author name (navigation property from seed data).
                query = query.Where(b =>
                    b.Author != null &&
                    b.Author.Name.Contains(author, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination: page 1 → Skip(0); clamp pageSize to at least 1 to avoid Take(0) surprises.
            var safePage = page < 1 ? 1 : page;
            var safePageSize = pageSize < 1 ? 10 : pageSize;

            var pageItems = query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(BookMapper.ToResponse)
                .ToList();

            return Task.FromResult(pageItems);
        }

        public Task<BookResponseDTO> GetByIdAsync(int id)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            return Task.FromResult(BookMapper.ToResponse(book));
        }

        public Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
        {
            var author = InMemoryStore.Authors.FirstOrDefault(a => a.Id == dto.AuthorId);
            if (author is null)
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} was not found.");
            }

            var book = BookMapper.ToEntity(dto);
            book.Id = InMemoryStore.Books.Count == 0
                ? 1
                : InMemoryStore.Books.Max(b => b.Id) + 1;
            book.Author = author;

            InMemoryStore.Books.Add(book);

            return Task.FromResult(BookMapper.ToResponse(book));
        }

        public Task UpdateAsync(int id, BookUpdateDTO dto)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            var author = InMemoryStore.Authors.FirstOrDefault(a => a.Id == dto.AuthorId);
            if (author is null)
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} was not found.");
            }

            BookMapper.ApplyUpdate(book, dto, author);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            InMemoryStore.Books.Remove(book);

            return Task.CompletedTask;
        }
    }
}
