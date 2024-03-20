namespace LibraryManagement.DTOs
{
    public class PublisherDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly PublishedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
