using Assessment.Data.Context;
using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Data.Repositories;
using Assessment.Data.Repositories.Interfaces;

namespace Assessment.Data.EFUnitOfWork
{
    public class EFUnitOfWork : IEFUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Images = new ImageRepository(_context);
        }

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IImageRepository Images { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
