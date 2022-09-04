using Assessment.Data.Entities;
using Assessment.Logic.Dtos.CategoryDtos;
using Assessment.Logic.Dtos.ValidationDtos;

namespace Assessment.Logic.ValidationServices.Interfaces
{
    public interface ICategoryValidationService
    {
        Task<ValidationResultDto> ValidateForCreate(CreateCategoryDto criteria);

        Task<ValidationResultDto> ValidateForGet(int id, Category category);

        Task<ValidationResultDto> ValidateForUpdate(int id, UpdateCategoryDto entity);
        Task<(Category Category, ValidationResultDto ValidationResult)> ValidateForName(string criteria);
    }
}
