namespace LibraryManagement.DTOs
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
    }
}
