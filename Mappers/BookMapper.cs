namespace Session1
{
    // TASK 3.2 — Static mapper (no AutoMapper). Keeps entity ↔ DTO conversion in one place.
    public static class BookMapper
    {
        // Maps a create DTO to a new Book entity. Caller assigns Id and Author navigation
        //kiunke jab humne book create ki toh humne id assign nahi kiya tha also author navigation property is not set yet (we only have authorId from the dto)
        public static Book ToEntity(BookCreateDTO dto)//this method will convert dto to entity
        {
            return new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                PageCount = dto.PageCount,
                AuthorId = dto.AuthorId
            };
        }

        // applies update DTO fields onto an existing tracked entity (in-memory list item).
        public static void ApplyUpdate(Book entity, BookUpdateDTO dto, Author author)
        {
            entity.Title = dto.Title;
            entity.Year = dto.Year;
            entity.PageCount = dto.PageCount;
            entity.AuthorId = dto.AuthorId;
            entity.Author = author;
        }

        // maps a Book entity to the API response record.
        public static BookResponseDTO ToResponse(Book entity)//this method will convert entity to dto
        //jab wapis controller ko bhejna hai toh hume entity ko dto me convert karna padega
        {
            return new BookResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Year = entity.Year,
                PageCount = entity.PageCount,
                AuthorId = entity.AuthorId,
                AuthorName = entity.Author?.Name ?? string.Empty
            };
        }
    }
}
