namespace Session1
{
    public class Book
    {
        public int Id { get; set; } //this will be primary key
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int PageCount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } //foreign key relationship
    }
}
