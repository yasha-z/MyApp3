namespace Session1
{
     public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>(); // one to many relationship, bcs one author can 
        // have many books, like a collection of books

        //collection navigation property, it will be used to load the books of an author
    }
}