namespace LibraryManagement.Domain.Entities
{
    public class Appuser
    {        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }        
    }
}
