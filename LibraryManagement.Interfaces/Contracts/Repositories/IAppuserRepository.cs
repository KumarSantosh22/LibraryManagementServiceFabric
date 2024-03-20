using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Interfaces.Contracts.Repositories
{
    public interface IAppuserRepository
    {
        Task<Appuser[]> GetAllAsync();

        Task<Appuser> GetAsync(Guid key);

        Task<Appuser> AddAsync(Appuser entity);

        Task<Appuser> UpdateAsync(Guid key, Appuser entity);

        Task DeleteAsync(Guid key);
    }
}
