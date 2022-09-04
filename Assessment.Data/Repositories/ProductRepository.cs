using Assessment.Data.Context;
using Assessment.Data.Entities;
using Assessment.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Product>> GetProductsByNameAsync(string name)
        {
            var products = _context.Products.Where(x => x.Name.Contains(name));

            return products;
        }

        public async Task<IQueryable<Product>> GetProductsByDescriptionAsync(string description)
        {
            var products = _context.Products.Where(x => x.Name.Contains(description));

            return products;
        }

        public async Task<IQueryable<Product>> GetProductsByCategoryAsync(int id)
        {
            var products = _context.Products.Where(x => x.CategoryId == id);

            return products;
        }
    }
}
