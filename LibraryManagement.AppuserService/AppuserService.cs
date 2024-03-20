using System.Fabric;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LibraryManagement.AppuserService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class AppuserService : StatefulService, IAppuserService
    {
        private IAppuserRepository _repository;

        public AppuserService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<AppuserDto> AddAsync(AppuserDto entity)
        {
            Appuser result = await _repository.AddAsync(entity.MapToEntity());
            return new AppuserDto(result);
        }

        public async Task DeleteAsync(Guid key)
        {
            await _repository.DeleteAsync(key);
        }

        public async Task<AppuserDto[]> GetAllAsync()
        {
            Appuser[]  result = await _repository.GetAllAsync();
            return result.Select(x => new AppuserDto(x)).ToArray();
        }

        public async Task<AppuserDto> GetAsync(Guid key)
        {
            Appuser result =  await _repository.GetAsync(key);
            return new AppuserDto(result);
        }

        public async Task<AppuserDto> UpdateAsync(Guid key, AppuserDto entity)
        {
            Appuser result = await _repository.UpdateAsync(key, entity.MapToEntity());
            return new AppuserDto(result);
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
            _repository = new FabricAppuserRepository(StateManager);
        }
    }
}
