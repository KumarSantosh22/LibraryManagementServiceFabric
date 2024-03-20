namespace LibraryManagement.Domain.Entities
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
