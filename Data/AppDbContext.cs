using Microsoft.EntityFrameworkCore;

namespace Session1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<BookTag> BookTags => Set<BookTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookTag>()
                .HasKey(bt => new { bt.BookId, bt.TagId });

            modelBuilder.Entity<BookTag>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(bt => bt.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookTag>()
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
