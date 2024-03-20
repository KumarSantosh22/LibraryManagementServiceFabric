using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LibraryManagement.Interfaces.Contracts.Services
{
    public interface IBookTransactionService: IService
    {
        Task<BookTransactionDto[]> GetAllAsync();

        Task<BookTransactionDto> GetAsync(int key);

        Task<BookTransactionDto> AddAsync(BookTransactionDto dto);

        Task<BookTransactionDto> UpdateAsync(int key, BookTransactionDto dto);

        Task DeleteAsync(int key);
    }
}
