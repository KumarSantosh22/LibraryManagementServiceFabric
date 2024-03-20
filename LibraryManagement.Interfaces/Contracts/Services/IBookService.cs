using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LibraryManagement.Interfaces.Contracts.Services
{
    public interface IBookService: IService
    {
        Task<BookDto[]> GetAllAsync();

        Task<BookDto> GetAsync(Guid key);

        Task<BookDto> AddAsync(BookDto dto);

        Task<BookDto> UpdateAsync(Guid key, BookDto dto);

        Task DeleteAsync(Guid key);
    }
}
