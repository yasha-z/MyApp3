using Microsoft.EntityFrameworkCore;

namespace Session1
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _db;

        public BookService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
        {
            var query = _db.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b =>
                    b.Author != null &&
                    b.Author.Name.Contains(author));
            }

            var safePage = page < 1 ? 1 : page;
            var safePageSize = pageSize < 1 ? 10 : pageSize;

            return await query
                .OrderBy(b => b.Id)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(b => BookMapper.ToResponse(b))
                .ToListAsync();
        }

        public async Task<BookResponseDTO> GetByIdAsync(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            return BookMapper.ToResponse(book);
        }

        public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
        {
            var authorExists = await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId);
            if (!authorExists)
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} was not found.");
            }

            var book = BookMapper.ToEntity(dto);
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            await _db.Entry(book).Reference(b => b.Author).LoadAsync();

            return BookMapper.ToResponse(book);
        }

        public async Task UpdateAsync(int id, BookUpdateDTO dto)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
            if (author is null)
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} was not found.");
            }

            BookMapper.ApplyUpdate(book, dto, author);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
        }
    }
}
