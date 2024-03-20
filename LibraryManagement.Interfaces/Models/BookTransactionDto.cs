using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Models
{
    public class BookTransactionDto
    {
        public int Id { get; set; }

        public Guid BookId { get; set; }

        public Guid IssuedTo { get; set; }

        public Guid IssuedBy { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime IssuedTill { get; set; }

        public DateTime ReturnedOn { get; set; }

        public double LateFine { get; set; }

        public bool IsActive { get; set; }

        public BookTransactionDto() { }

        public BookTransactionDto(BookTransaction book)
        {
            Id = book.Id;
            BookId = book.BookId;
            IssuedTo = book.IssuedTo;
            IssuedBy = book.IssuedBy;
            IssuedOn = book.IssuedOn;
            IssuedTill = book.IssuedTill;
            ReturnedOn = book.ReturnedOn;
            LateFine = book.LateFine;
            IsActive = book.IsActive;
        }

        public BookTransaction MapToEntity() => new BookTransaction
        {
            Id = Id,
            BookId = BookId,
            IssuedTo = BookId,
            IssuedBy = BookId,
            IssuedTill = IssuedTill,
            ReturnedOn = ReturnedOn,
            LateFine = LateFine,
            IsActive = IsActive
        };
    }
}
