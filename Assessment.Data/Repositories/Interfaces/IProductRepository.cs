using Assessment.Data.Entities;

namespace Assessment.Data.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IQueryable<Product>> GetProductsByNameAsync(string name);
        Task<IQueryable<Product>> GetProductsByDescriptionAsync(string description);
        Task<IQueryable<Product>> GetProductsByCategoryAsync(int id);
    }
}
