using System.Linq;

namespace Session1
{
    // TASK 3.4 — Service uses BookMapper and works with DTOs; entity access stays here.
    public class BookService : IBookService
    {
        public Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
        {
            // start from full in-memory list, then apply optional author filter.
            IEnumerable<Book> query = InMemoryStore.Books;

            if (!string.IsNullOrWhiteSpace(author))//agar author null ya empty nahi hai toh filter apply karenge
            {
                
                query = query.Where(b =>
                    b.Author != null &&
                    b.Author.Name.Contains(author, StringComparison.OrdinalIgnoreCase));//case-insensitive search
            }

            // Pagination: page 1 → Skip(0); clamp pageSize to at least 1 to avoid Take(0) surprises.
            var safePage = page < 1 ? 1 : page; //if page is less than 1, then set it to 1
            var safePageSize = pageSize < 1 ? 10 : pageSize;  //if pageSize is less than 1, then set it to 10

            var pageItems = query
                .Skip((safePage - 1) * safePageSize) //eg page is 2 and pageSize is 10, then skip 10 items : 2-1 = 1, 1*10 = 10
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

            var book = BookMapper.ToEntity(dto); //here we are converting dto to entity using mapper
//this is to generate a new id for the book, if there are no books in the store, then id will be 1, otherwise it will be max id + 1
//kiunke obviously dto me id nahi hota, toh hume manually id generate karna padega
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
