using System.Fabric;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LibraryManagement.BookCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class BookCatalog : StatefulService, IBookService
    {
        private IBookRepository _repository;
        public BookCatalog(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<BookDto> AddAsync(BookDto dto)
        {

            Book book =  await _repository.AddAsync(dto.MapToEntity());
            return new BookDto(book);
        }

        public async Task DeleteAsync(Guid key)
        {
            await _repository.DeleteAsync(key);
        }

        public async Task<BookDto[]> GetAllAsync()
        {
            Book[] books = await _repository.GetAllAsync();
            return books.Select(book => new BookDto(book)).ToArray();
        }

        public async Task<BookDto> GetAsync(Guid key)
        {
            Book book = await _repository.GetAsync(key);
            return new BookDto(book);
        }

        public async Task<BookDto> UpdateAsync(Guid key, BookDto dto)
        {
            Book book = await _repository.UpdateAsync(key, dto.MapToEntity());
            return new BookDto(book);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return
            [
                new  ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
            ];
        }
                
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repository = new FabricBookRepository(StateManager);
        }
    }
}
