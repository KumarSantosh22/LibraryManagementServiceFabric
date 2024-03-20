using System.Fabric;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LibraryManagement.PublisherService
{
    internal sealed class PublisherService : StatefulService, IPublisherService
    {
        private IPublisherRepository _repository;

        public PublisherService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<PublisherDto> AddAsync(PublisherDto dto)
        {
            Publisher publisher = await _repository.AddAsync(dto.MapToEntity());
            return new PublisherDto(publisher);
        }

        public async Task DeleteAsync(Guid key)
        {
            await _repository.DeleteAsync(key);
        }

        public async Task<PublisherDto[]> GetAllAsync()
        {
            Publisher[] publishers = await _repository.GetAllAsync();
            return publishers.Select(x => new PublisherDto(x)).ToArray();
        }

        public async Task<PublisherDto> GetAsync(Guid key)
        {
            Publisher publisher = await _repository.GetAsync(key);
            return new PublisherDto(publisher);
        }

        public async Task<PublisherDto> UpdateAsync(Guid key, PublisherDto dto)
        {
            Publisher publisher = await _repository.UpdateAsync(key, dto.MapToEntity());
            return new PublisherDto(publisher); 
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return [
                    new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
                ];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repository = new FabricPublisherRepository(StateManager);
        }
    }
}
