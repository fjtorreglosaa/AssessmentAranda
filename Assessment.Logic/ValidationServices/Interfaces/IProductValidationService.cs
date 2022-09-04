using Assessment.Data.Entities;
using Assessment.Logic.Dtos.ProductDtos;
using Assessment.Logic.Dtos.ValidationDtos;

namespace Assessment.Logic.ValidationServices.Interfaces
{
    public interface IProductValidationService
    {
        Task<ValidationResultDto> ValidateForCreate(CreateProductDto criteria);
        Task<ValidationResultDto> ValidateForGet(int id, Product entity);
        Task<ValidationResultDto> ValidateForUpdate(int id, UpdateProductDto criteria);
    }
}
