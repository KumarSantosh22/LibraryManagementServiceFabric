using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace LibraryManagement.PublisherService
{
    internal class FabricPublisherRepository : IPublisherRepository
    {
        private readonly IReliableStateManager _stateManager;

        public FabricPublisherRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Publisher> AddAsync(Publisher entity)
        {
            IReliableDictionary<Guid, Publisher> publishers = await GetPublishersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                bool isSucceed = await publishers.TryAddAsync(tx, entity.Id, entity);
                if(!isSucceed)
                {
                    throw new Exception("Not able to add data.");
                }
                await tx.CommitAsync();
            }

            return entity;
        }

        public async Task DeleteAsync(Guid key)
        {
            IReliableDictionary<Guid, Publisher> publishers = await GetPublishersAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
               ConditionalValue<Publisher> publisher = await publishers.TryRemoveAsync(tx, key);                
                await tx.CommitAsync();
            }

        }

        public async Task<Publisher[]> GetAllAsync()
        {
            List<Publisher> result = new List<Publisher>();
            IReliableDictionary<Guid, Publisher> publishers = await GetPublishersAsync();
            
            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Publisher>> allPublishers = await publishers.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using(Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Publisher>> enumerator  = allPublishers.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }

            return result.ToArray();
        }

        public async Task<Publisher> GetAsync(Guid key)
        {
            ConditionalValue<Publisher> publisher;
            IReliableDictionary<Guid, Publisher> publishers = await GetPublishersAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                publisher = await publishers.TryGetValueAsync(tx, key);
                await tx.CommitAsync();
            }

            return publisher.Value;
        }

        public async Task<Publisher> UpdateAsync(Guid key, Publisher entity)
        {
            Publisher publisher;
            IReliableDictionary<Guid, Publisher> publishers = await GetPublishersAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                publisher = await publishers.AddOrUpdateAsync(tx, key, entity, (id, value) => value = entity);
                await tx.CommitAsync();
            }

            return publisher;
        }

        private async Task<IReliableDictionary<Guid, Publisher>> GetPublishersAsync()
        {
            IReliableDictionary<Guid, Publisher> publishers = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Publisher>>("publishers");

            return publishers;
        }
    }
}
