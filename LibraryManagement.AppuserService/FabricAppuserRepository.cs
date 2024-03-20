using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace LibraryManagement.AppuserService
{
    internal class FabricAppuserRepository : IAppuserRepository
    {
        private IReliableStateManager _stateManager;

        public FabricAppuserRepository(
            IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Appuser> AddAsync(Appuser entity)
        {
            IReliableDictionary<Guid, Appuser> appusers = await GetUsersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                await appusers.AddAsync(tx, entity.Id, entity);
                await tx.CommitAsync();
            }

            return entity;
        }

        public async Task DeleteAsync(Guid key)
        {
            IReliableDictionary<Guid, Appuser> appusers = await GetUsersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Appuser> appuser =  await appusers.TryRemoveAsync(tx, key);
                await tx.CommitAsync();
            }

        }

        public async Task<Appuser[]> GetAllAsync()
        {
            List<Appuser> result = new();
            IReliableDictionary<Guid, Appuser> appusers = await GetUsersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Appuser>> allUsers = await appusers.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using(Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Appuser>> enumerator  = allUsers.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Appuser> item = enumerator.Current;
                        result.Add(item.Value);
                    }
                }
            }

            return result.ToArray();
        }

        public async Task<Appuser> GetAsync(Guid key)
        {
            IReliableDictionary<Guid, Appuser> appusers = await GetUsersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Appuser> result =  await appusers.TryGetValueAsync(tx, key);
                return result.Value;
            }
        }

        public async Task<Appuser> UpdateAsync(Guid key, Appuser entity)
        {
            Appuser result;
            IReliableDictionary<Guid, Appuser> appusers = await GetUsersAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                result = await appusers.AddOrUpdateAsync(tx,
                                                    key,
                                                    entity,
                                                    (id, value) => entity);
                await tx.CommitAsync();
            }

            return result;
        }

        private async Task<IReliableDictionary<Guid, Appuser>> GetUsersAsync()
        {
            IReliableDictionary<Guid, Appuser> appusers = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Appuser>>("appusers");
            return appusers;
        }
    }
}
