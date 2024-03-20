using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using LibraryManagement.AppuserActor.Interfaces;

namespace LibraryManagement.AppuserActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class AppuserActor : Actor, IAppuserActor
    {
        public AppuserActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        public async Task AddToBasket(BasketItem item)
        {
            await StateManager.AddOrUpdateStateAsync(item.BookId.ToString(), item.Quantity, (id, value) => value + item.Quantity);
        }

        public async Task ClearBasket()
        {
            IEnumerable<string> states = await StateManager.GetStateNamesAsync();

            foreach (string state in states)
            {
                await StateManager.RemoveStateAsync(state);
            }
        }

        public async Task<BasketItem[]> GetBasket()
        {
            List<BasketItem> basketItems = new();
            IEnumerable<string> states = await StateManager.GetStateNamesAsync();
            foreach(string state in states)
            {
                int qty = await StateManager.GetStateAsync<int>(state);
                basketItems.Add(new BasketItem { BookId = new Guid(state), Quantity = qty });
            }

            return basketItems.ToArray();
        }                
    }
}
