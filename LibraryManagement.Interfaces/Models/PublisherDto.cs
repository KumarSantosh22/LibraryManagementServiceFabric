using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Models
{
    public class PublisherDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsActive { get; set; }

        public PublisherDto() { }

        public PublisherDto(Publisher publisher)
        {
            Id = publisher.Id;
            Name = publisher.Name;
            PublishedOn = publisher.PublishedOn;
            IsActive = publisher.IsActive;
        }

        public Publisher MapToEntity() => new Publisher
        {
            Id = Id,
            Name = Name,
            PublishedOn = PublishedOn,
            IsActive = IsActive
        };
    }
}
