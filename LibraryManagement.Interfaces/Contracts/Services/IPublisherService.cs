using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LibraryManagement.Interfaces.Contracts.Services
{
    public interface IPublisherService: IService
    {
        Task<PublisherDto[]> GetAllAsync();

        Task<PublisherDto> GetAsync(Guid key);

        Task<PublisherDto> AddAsync(PublisherDto dto);

        Task<PublisherDto> UpdateAsync(Guid key, PublisherDto dto);

        Task DeleteAsync(Guid key);
    }
}
