namespace Assessment.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task Add(T entity);
        Task AddRange(List<T> entities);
        void Delete(T entity);
        void Update(T entity);
    }
}
