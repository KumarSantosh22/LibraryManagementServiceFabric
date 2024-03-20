using System.Fabric;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LibraryManagement.AuthorService
{
    internal sealed class AuthorService : StatefulService, IAuthorService
    {
        private IAuthorRepository _repository;

        public AuthorService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<AuthorDto> AddAsync(AuthorDto dto)
        {
            Author author = await _repository.AddAsync(dto.MapToEntity());
            
            return new AuthorDto(author);
        }

        public async Task DeleteAsync(Guid key)
        {
            await _repository.DeleteAsync(key);
        }

        public async Task<AuthorDto[]> GetAllAsync()
        {
            Author[] authors = await _repository.GetAllAsync();
            
            return authors.Select(author => new AuthorDto(author)).ToArray();
        }

        public async Task<AuthorDto> GetAsync(Guid key)
        {
            Author author = await _repository.GetAsync(key);
            return new AuthorDto(author);
        }

        public async Task<AuthorDto> UpdateAsync(Guid key, AuthorDto dto)
        {
            Author author = await _repository.UpdateAsync(key, dto.MapToEntity());

            return new AuthorDto(author);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return [
                        new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
                    ];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repository = new FabricAuthorRepository(StateManager);
        }
    }
}
