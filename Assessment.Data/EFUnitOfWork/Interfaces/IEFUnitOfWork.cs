using Assessment.Data.Repositories.Interfaces;

namespace Assessment.Data.EFUnitOfWork.Interfaces
{
    public interface IEFUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IImageRepository Images { get; }
        Task<int> Complete();
    }
}
