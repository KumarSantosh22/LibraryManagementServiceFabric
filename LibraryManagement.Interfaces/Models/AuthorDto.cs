using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Models
{
    public class AuthorDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public AuthorDto() { }

        public AuthorDto(Author author)
        {
            Id = author.Id;
            Name = author.Name;
            Description = author.Description;
            IsActive = author.IsActive;
        }

        public Author MapToEntity() => new Author
        {
            Id = Id,
            Name = Name,
            Description = Description,
            IsActive = IsActive
        };
    }
}
