using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Models
{
    public class AppuserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public AppuserDto() { }

        public AppuserDto(Appuser appuser)
        {
            Id = appuser.Id;
            Name = appuser.Name;
            Email = appuser.Email;
            PhoneNumber = appuser.PhoneNumber;
            DateOfBirth = appuser.DateOfBirth;
            IsActive = appuser.IsActive;
        }

        public Appuser MapToEntity() => new Appuser
        {
            Id = Id,
            Name = Name,
            Email = Email,
            PhoneNumber = PhoneNumber,
            DateOfBirth = DateOfBirth,
            IsActive = IsActive,
            Password = "1234567890"
        };
    }
}
