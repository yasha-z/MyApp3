namespace Session1
{
    // TASK 2.5 — Custom exception for missing books
    public class BookNotFoundException : Exception
    {
        public int BookId { get; }

        public BookNotFoundException(int bookId)
            : base($"Book with ID {bookId} was not found.")
        {
            BookId = bookId;
        }
    }
}
