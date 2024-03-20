using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LibraryManagement.Interfaces.Contracts.Services
{
    public interface IAppuserService: IService
    {
        Task<AppuserDto[]> GetAllAsync();

        Task<AppuserDto> GetAsync(Guid key);

        Task<AppuserDto> AddAsync(AppuserDto entity);

        Task<AppuserDto> UpdateAsync(Guid key, AppuserDto entity);

        Task DeleteAsync(Guid key);
    }
}
