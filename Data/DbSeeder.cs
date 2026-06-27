using Microsoft.EntityFrameworkCore;

namespace Session1
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.Authors.AnyAsync())
            {
                return;
            }

            var author1 = new Author { Id = 1, Name = "J.K. Rowling" };
            var author2 = new Author { Id = 2, Name = "J.R.R. Tolkien" };
            var author3 = new Author { Id = 3, Name = "George R.R. Martin" };

            db.Authors.AddRange(author1, author2, author3);

            db.Books.AddRange(
                new Book { Id = 1, Title = "Harry Potter 1", Year = 1997, PageCount = 300, AuthorId = 1 },
                new Book { Id = 2, Title = "Harry Potter 2", Year = 1998, PageCount = 350, AuthorId = 1 },
                new Book { Id = 3, Title = "Harry Potter 3", Year = 1999, PageCount = 430, AuthorId = 1 },
                new Book { Id = 4, Title = "The Hobbit", Year = 1937, PageCount = 310, AuthorId = 2 },
                new Book { Id = 5, Title = "The Fellowship of the Ring", Year = 1954, PageCount = 420, AuthorId = 2 },
                new Book { Id = 6, Title = "The Two Towers", Year = 1954, PageCount = 350, AuthorId = 2 },
                new Book { Id = 7, Title = "A Game of Thrones", Year = 1996, PageCount = 690, AuthorId = 3 },
                new Book { Id = 8, Title = "A Clash of Kings", Year = 1998, PageCount = 768, AuthorId = 3 });

            db.Tags.AddRange(
                new Tag { Id = 1, Name = "Fantasy" },
                new Tag { Id = 2, Name = "Magic" },
                new Tag { Id = 3, Name = "Adventure" },
                new Tag { Id = 4, Name = "Dragons" });

            db.BookTags.AddRange(
                new BookTag { BookId = 1, TagId = 1 },
                new BookTag { BookId = 1, TagId = 2 },
                new BookTag { BookId = 4, TagId = 1 },
                new BookTag { BookId = 4, TagId = 3 },
                new BookTag { BookId = 7, TagId = 1 },
                new BookTag { BookId = 7, TagId = 4 });

            await db.SaveChangesAsync();
        }
    }
}
