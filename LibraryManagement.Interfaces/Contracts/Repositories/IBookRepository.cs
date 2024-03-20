using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Contracts.Repositories
{
    public interface IBookRepository: IRepository<Book, Guid>
    {
    }
}
