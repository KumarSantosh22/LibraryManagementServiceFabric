using System.Fabric;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LibraryManagement.BookTransactionService
{
    internal sealed class BookTransactionService : StatefulService, IBookTransactionService
    {
        private IBookTransactionRepository _repository;
        public BookTransactionService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<BookTransactionDto> AddAsync(BookTransactionDto dto)
        {
            BookTransaction bookTransaction = await _repository.AddAsync(dto.MapToEntity());
            return new BookTransactionDto(bookTransaction);
        }

        public async Task DeleteAsync(int key)
        {
            await _repository.DeleteAsync(key);
        }

        public async Task<BookTransactionDto[]> GetAllAsync()
        {
            BookTransaction[] bookTransactions = await _repository.GetAllAsync();
            return bookTransactions.Select(bookTransaction => new BookTransactionDto(bookTransaction)).ToArray();
        }

        public async Task<BookTransactionDto> GetAsync(int key)
        {
            BookTransaction bookTransaction = await _repository.GetAsync(key);
            return new BookTransactionDto(bookTransaction);
        }

        public async Task<BookTransactionDto> UpdateAsync(int key, BookTransactionDto dto)
        {
            BookTransaction bookTransaction = await _repository.UpdateAsync(key, dto.MapToEntity());
            return new BookTransactionDto(bookTransaction);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return [
                    new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
                ];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repository = new FabricBookTransactionRepository(StateManager);
        }
    }
}
