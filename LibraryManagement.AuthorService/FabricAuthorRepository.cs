using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace LibraryManagement.AuthorService
{
    internal class FabricAuthorRepository : IAuthorRepository
    {
        private IReliableStateManager _stateManager;

        public FabricAuthorRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Author> AddAsync(Author entity)
        {
            Author author;
            IReliableDictionary<Guid, Author> authors =  await GetAuthorsAsync();

            using (ITransaction tx  = _stateManager.CreateTransaction())
            {
                author = await authors.AddOrUpdateAsync(tx, entity.Id,  entity, (id, value)=> value = entity);
                await tx.CommitAsync();
            }

            return author;
        }

        public async Task DeleteAsync(Guid key)
        {
            IReliableDictionary<Guid, Author> authors = await GetAuthorsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await authors.TryRemoveAsync(tx, key);
                await tx.CommitAsync();
            }
        }

        public async Task<Author[]> GetAllAsync()
        {
            List<Author> result = new List<Author>();
            IReliableDictionary<Guid, Author> authors = await GetAuthorsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Author>> allAuthors = await authors.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using(Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Author>> enumerator  =  allAuthors.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }

            return result.ToArray();
        }

        public async Task<Author> GetAsync(Guid key)
        {
            ConditionalValue<Author> author;
            IReliableDictionary<Guid, Author> authors = await GetAuthorsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                author = await authors.TryGetValueAsync(tx, key);
            }

            return author.Value;
        }

        public async Task<Author> UpdateAsync(Guid key, Author entity)
        {
            Author author;
            IReliableDictionary<Guid, Author> authors = await GetAuthorsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                author = await authors.AddOrUpdateAsync(tx, key, entity, (id, value) => value = entity);
                await tx.CommitAsync();
            }

            return author;
        }

        private async Task<IReliableDictionary<Guid, Author>> GetAuthorsAsync()
        {
            IReliableDictionary<Guid, Author> authors = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Author>>("authors");

            return authors;
        }
    }
}
