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
                .Include(b => b.Author)//its equivalent to a join
                .AsNoTracking()// DONT REMEMBER THIS! it tells EF that im not gonna update these only looking
                .AsQueryable();//means im not done yet ill add more filters to this query later

            if (!string.IsNullOrWhiteSpace(author))//if author exists
            {
                query = query.Where(b =>
                    b.Author != null &&
                    b.Author.Name.Contains(author));//zaroori nahin puri string match ho
            }

            var safePage = page < 1 ? 1 : page;//if page is less than 1 then set it to 1
            var safePageSize = pageSize < 1 ? 10 : pageSize;

            return await query
                .OrderBy(b => b.Id)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .Select(b => BookMapper.ToResponse(b))
                .ToListAsync();//sql finally runs now
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
        {//ab aik book create karne se pehle ye check karenge ki author exists karta hai ya nahi
            var authorExists = await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId);
            if (!authorExists)
            {
                throw new ArgumentException($"Author with ID {dto.AuthorId} was not found.");
            }
//ab jo dto aya hai usko entity me convert karenge aur db me save karenge
            var book = BookMapper.ToEntity(dto);
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            await _db.Entry(book).Reference(b => b.Author).LoadAsync();//

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
