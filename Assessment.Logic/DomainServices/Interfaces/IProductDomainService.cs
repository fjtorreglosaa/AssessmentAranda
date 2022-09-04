using Assessment.Data.Entities;
using Assessment.Logic.Dtos.ProductDtos;
using Assessment.Logic.Dtos.ValidationDtos;

namespace Assessment.Logic.DomainServices.Interfaces
{
    public interface IProductDomainService
    {
        Task<(ProductDto dto, ValidationResultDto ValidationResult)> CreateProduct(CreateProductDto criteria);
        Task<IQueryable<Product>> GetProducts();
        Task<IQueryable<Product>> GetProductsByName(string criteria);
        Task<IQueryable<Product>> GetProductsByDescription(string criteria);
        Task<(IQueryable<Product> Queryable, ValidationResultDto ValidationResult)> GetProductsByCategory(string criteria);
        Task<(ProductDto dto, ValidationResultDto ValidationResult)> GetProductById(int id);
        Task<(ProductDto dto, ValidationResultDto ValidationResult)> UpdateProductById(int id, UpdateProductDto criteria);
        Task<(ProductDto dto, ValidationResultDto ValidationResult)> DeleteProductById(int id);
    }
}
