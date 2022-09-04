using Assessment.Data.Context;
using Assessment.Data.Entities;
using Assessment.Data.Repositories.Interfaces;

namespace Assessment.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
