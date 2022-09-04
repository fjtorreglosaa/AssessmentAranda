using Assessment.Data.Entities;
using Assessment.Logic.Dtos.CategoryDtos;
using Assessment.Logic.Dtos.ValidationDtos;

namespace Assessment.Logic.DomainServices.Interfaces
{
    public interface ICategoryDomainService
    {
        Task<(CategoryDto dto, ValidationResultDto ValidationResult)> CreateCategory(CreateCategoryDto criteria);

        Task<IQueryable<Category>> GetAllAsync();

        Task<(CategoryDto dto, ValidationResultDto ValidationResult)> GetCategoryByIdAsync(int id);

        Task<(CategoryDto dto, ValidationResultDto ValidationResult)> UpdateCategoryByIdAsync(int id, UpdateCategoryDto criteria);

        Task<(CategoryDto dto, ValidationResultDto ValidationResult)> DeleteCategoryById(int id);
    }
}
