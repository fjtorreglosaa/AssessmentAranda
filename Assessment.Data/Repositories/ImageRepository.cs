using Assessment.Data.Context;
using Assessment.Data.Entities;
using Assessment.Data.Repositories.Interfaces;

namespace Assessment.Data.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
