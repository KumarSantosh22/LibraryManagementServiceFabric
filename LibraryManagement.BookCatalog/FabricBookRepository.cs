using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace LibraryManagement.BookCatalog
{
    internal class FabricBookRepository : IBookRepository
    {
        private readonly IReliableStateManager _stateManager;

        public FabricBookRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<Book> AddAsync(Book entity)
        {
            IReliableDictionary<Guid, Book> books = await GetBooksAsync();
            
            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                await books.AddAsync(tx, entity.Id, entity);
                await tx.CommitAsync();
            }

            return entity;
        }

        public async Task DeleteAsync(Guid key)
        {
            IReliableDictionary<Guid, Book> books = await GetBooksAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Book> book = await books.TryRemoveAsync(tx, key);
                await tx.CommitAsync();
            }
        }

        public async Task<Book[]> GetAllAsync()
        {
            List<Book> result = new List<Book>();
            IReliableDictionary<Guid, Book> books = await GetBooksAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Book>> allBooks = await books.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using(Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Book>> enumerator = allBooks.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }

            return result.ToArray();
        }

        public async Task<Book> GetAsync(Guid key)
        {
            ConditionalValue<Book> book;
            IReliableDictionary<Guid, Book> books = await GetBooksAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                book = await books.TryGetValueAsync(tx, key);
                await tx.CommitAsync();

            }

            return book.Value;
        }

        public async Task<Book> UpdateAsync(Guid key, Book entity)
        {
            Book book;
            IReliableDictionary<Guid, Book> books = await GetBooksAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                book =  await books.AddOrUpdateAsync(
                    tx, key, entity,
                    (id, value) => entity);
                await tx.CommitAsync();                
            }

            return book;
        }

        private async Task<IReliableDictionary<Guid, Book>> GetBooksAsync()
        {
            IReliableDictionary<Guid, Book> books = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Book>>("books");
            return books;
        }
    }
}
