using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? PublisherId { get; set; }

        public bool IsActive { get; set; }

        public BookDto() { }

        public BookDto(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Description = book.Description;
            Price = book.Price;
            Quantity = book.Quantity;
            AuthorId = book.AuthorId;
            PublisherId = book.PublisherId;
            IsActive = book.IsActive;
        }

        public Book MapToEntity() => new Book
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Price = Price,
            Quantity = Quantity,
            AuthorId = AuthorId,
            PublisherId= PublisherId,
            IsActive = IsActive,
        };
    }
}
