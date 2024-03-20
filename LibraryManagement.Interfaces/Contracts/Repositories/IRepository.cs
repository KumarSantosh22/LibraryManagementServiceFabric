namespace LibraryManagement.Interfaces.Contracts.Repositories
{
    public interface IRepository<T, TKey>
    {
        Task<T[]> GetAllAsync();

        Task<T> GetAsync(TKey key);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(TKey key, T entity);

        Task DeleteAsync(TKey key);
    }
}
