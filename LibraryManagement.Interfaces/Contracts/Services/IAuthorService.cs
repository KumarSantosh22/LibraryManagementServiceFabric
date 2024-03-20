using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LibraryManagement.Interfaces.Contracts.Services
{
    public interface IAuthorService: IService
    {
        Task<AuthorDto[]> GetAllAsync();

        Task<AuthorDto> GetAsync(Guid key);

        Task<AuthorDto> AddAsync(AuthorDto dto);

        Task<AuthorDto> UpdateAsync(Guid key, AuthorDto dto);

        Task DeleteAsync(Guid key);
    }
}
