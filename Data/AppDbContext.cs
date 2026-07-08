using Microsoft.EntityFrameworkCore;

namespace Session1
{

    //this class represents the database context for the application
    // like the connection to the database and what tables there are in the database
    public class AppDbContext : DbContext//dbcontext knows how to talk to the database and it has methods like savechanges() to save changes to the database and update etc
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)// passing the details of the database connection to the base class constructor
        {
        }


//dbset represents a table in the db
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<BookTag> BookTags => Set<BookTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)//builds the model like how many tables there are, the rs btw them etc 
        {
            modelBuilder.Entity<BookTag>()
                .HasKey(bt => new { bt.BookId, bt.TagId });//composite primary key for the BookTag table

            modelBuilder.Entity<BookTag>()//one book can have many tags and one tag can be associated with many books
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(bt => bt.BookId)
                .OnDelete(DeleteBehavior.Cascade);//if a book is deleted then all the tags associated with that book will also be deleted

            modelBuilder.Entity<BookTag>()
                .HasOne<Tag>()//one tag can be associated with many books and one book can have many tags
                .WithMany()
                .HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)//one book has one author and one author can have many books
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);//if an author is deleted then all the books associated with that author will not be deleted, 
                //instead the delete operation will be restricted
        }
    }
}
