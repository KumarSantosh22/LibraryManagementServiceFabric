using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Contracts.Repositories
{
    public interface IAuthorRepository: IRepository<Author, Guid>
    {
    }
}
