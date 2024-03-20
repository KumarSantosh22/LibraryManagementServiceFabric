namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
                
        public int Quantity { get; set; }

        public double Price { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? PublisherId { get; set; }

        public bool IsActive { get; set; }
    }
}
