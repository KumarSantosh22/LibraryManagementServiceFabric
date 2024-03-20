using LibraryManagement.Domain.Entities;

namespace LibraryManagement.AppuserActor.Interfaces
{
    public class BasketItem
    {
        public Guid BookId { get; set; }

        public int Quantity { get; set; }
    }
}
