using LibraryManagement.Domain.Entities;
using LibraryManagement.Interfaces.Contracts.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace LibraryManagement.BookTransactionService
{
    internal class FabricBookTransactionRepository : IBookTransactionRepository
    {
        private readonly IReliableStateManager _stateManager;

        public FabricBookTransactionRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<BookTransaction> AddAsync(BookTransaction entity)
        {
            IReliableDictionary<int, BookTransaction> transactions = await GetTransactionsAsync();

            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                await transactions.AddAsync(tx, entity.Id, entity);
                await tx.CommitAsync();
            }

            return entity;
        }

        public async Task DeleteAsync(int key)
        {
            IReliableDictionary<int, BookTransaction> transactions = await GetTransactionsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<BookTransaction> transaction = await transactions.TryRemoveAsync(tx, key);
                await tx.CommitAsync();
            }
        }

        public async Task<BookTransaction[]> GetAllAsync()
        {
            List<BookTransaction> result = new List<BookTransaction>();
            IReliableDictionary<int, BookTransaction> transactions = await GetTransactionsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<int, BookTransaction>> allTransactions = await transactions.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using(Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<int, BookTransaction>> enumerator  = allTransactions.GetAsyncEnumerator())
                {
                    while(await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        result.Add(enumerator.Current.Value);
                    }
                }
            }

            return result.ToArray();
        }

        public async Task<BookTransaction> GetAsync(int key)
        {
            ConditionalValue<BookTransaction> transaction;
            IReliableDictionary<int, BookTransaction> transactions = await GetTransactionsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                transaction = await transactions.TryGetValueAsync(tx, key);
            }

            return transaction.Value;
        }

        public async Task<BookTransaction> UpdateAsync(int key, BookTransaction entity)
        {
            BookTransaction bookTransaction;
            IReliableDictionary<int, BookTransaction> transactions = await GetTransactionsAsync();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                bookTransaction = await transactions.AddOrUpdateAsync(tx, key, entity, (id, value) => value = entity);
                await tx.CommitAsync();
            }

            return bookTransaction;
        }

        private async Task<IReliableDictionary<int, BookTransaction>> GetTransactionsAsync()
        {
            IReliableDictionary<int, BookTransaction> transactions = await _stateManager.GetOrAddAsync<IReliableDictionary<int, BookTransaction>>("bookTransactions");

            return transactions;
        }
    }
}
