namespace LibraryManagement.DTOs
{
    public class AppuserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool IsActive { get; set; }
    }
}
