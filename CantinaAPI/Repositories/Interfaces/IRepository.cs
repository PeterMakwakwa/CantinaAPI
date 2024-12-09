namespace CantinaAPI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
