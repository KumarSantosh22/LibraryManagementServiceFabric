using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace LibraryManagement.AppuserActor.Interfaces
{
    public interface IAppuserActor : IActor
    {        
        Task AddToBasket(BasketItem item);

        Task<BasketItem[]> GetBasket();

        Task ClearBasket();
    }
}
