using System.Collections.Generic;
namespace Session1
{

    // DATABASE (this is fake not actual db)
    public static class InMemoryStore
    {
        public static List<Author> Authors { get; set; } = new();
        public static List<Book> Books { get; set; } = new();
        public static List<Tag> Tags { get; set; } = new();
        public static List<BookTag> BookTags { get; set; } = new();

        public static void SeedData()
        {
            var author1 = new Author { Id = 1, Name = "J.K. Rowling" };
            var author2 = new Author { Id = 2, Name = "J.R.R. Tolkien" };
            var author3 = new Author { Id = 3, Name = "George R.R. Martin" };

            Authors.AddRange(new[] { author1, author2, author3 });

//create the array from these items : new[], pass the array to addrange func then addrange adds multiple items into a list at once
            Books.AddRange(new[]
            {
                new Book { Id = 1, Title = "Harry Potter 1", Year = 1997, PageCount = 300, AuthorId = 1, Author = author1 },
                new Book { Id = 2, Title = "Harry Potter 2", Year = 1998, PageCount = 350, AuthorId = 1, Author = author1 },
                new Book { Id = 3, Title = "Harry Potter 3", Year = 1999, PageCount = 430, AuthorId = 1, Author = author1 },
                new Book { Id = 4, Title = "The Hobbit", Year = 1937, PageCount = 310, AuthorId = 2, Author = author2 },
                new Book { Id = 5, Title = "The Fellowship of the Ring", Year = 1954, PageCount = 420, AuthorId = 2, Author = author2 },
                new Book { Id = 6, Title = "The Two Towers", Year = 1954, PageCount = 350, AuthorId = 2, Author = author2 },
                new Book { Id = 7, Title = "A Game of Thrones", Year = 1996, PageCount = 690, AuthorId = 3, Author = author3 },
                new Book { Id = 8, Title = "A Clash of Kings", Year = 1998, PageCount = 768, AuthorId = 3, Author = author3 }
            });

            Tags.AddRange(new[]
            {
                new Tag { Id = 1, Name = "Fantasy" },
                new Tag { Id = 2, Name = "Magic" },
                new Tag { Id = 3, Name = "Adventure" },
                new Tag { Id = 4, Name = "Dragons" }
            });

            BookTags.AddRange(new[]
            {
                new BookTag { BookId = 1, TagId = 1 },
                new BookTag { BookId = 1, TagId = 2 },
                new BookTag { BookId = 4, TagId = 1 },
                new BookTag { BookId = 4, TagId = 3 },
                new BookTag { BookId = 7, TagId = 1 },
                new BookTag { BookId = 7, TagId = 4 }
            });
        }
    }
}