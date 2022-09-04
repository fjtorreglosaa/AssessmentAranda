using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Data.Entities;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.CategoryDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.Extensions;
using Assessment.Logic.ValidationServices.Interfaces;

namespace Assessment.Logic.DomainServices
{
    public class CategoryDomainService : ICategoryDomainService
    {
        private readonly IEFUnitOfWork _unitOfWork;
        private readonly ICategoryValidationService _categoryValidationService;

        public CategoryDomainService(IEFUnitOfWork unitOfWork, ICategoryValidationService categoryValidationService)
        {
            _unitOfWork = unitOfWork;
            _categoryValidationService = categoryValidationService;
        }

        public async Task<(CategoryDto dto, ValidationResultDto ValidationResult)> CreateCategory(CreateCategoryDto criteria)
        {
            var validationResult = await _categoryValidationService.ValidateForCreate(criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var entity = criteria.ConvertToEntity();

            await _unitOfWork.Categories.Add(entity);
            await _unitOfWork.Complete();

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<IQueryable<Category>> GetAllAsync()
        {
            var queryable = _unitOfWork.Categories.GetAll();

            return queryable;
        }

        public async Task<(CategoryDto dto, ValidationResultDto ValidationResult)> GetCategoryByIdAsync(int id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            var validationResult = await _categoryValidationService.ValidateForGet(id, entity);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<(CategoryDto dto, ValidationResultDto ValidationResult)> UpdateCategoryByIdAsync(int id, UpdateCategoryDto criteria)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            var validationResult = await _categoryValidationService.ValidateForGet(id, entity);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            validationResult = await _categoryValidationService.ValidateForUpdate(id, criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            entity.Name = criteria.Name;

            await _unitOfWork.Complete();

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<(CategoryDto dto, ValidationResultDto ValidationResult)> DeleteCategoryById(int id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            var validationResultDto = await _categoryValidationService.ValidateForGet(id, entity);

            if (validationResultDto.Conditions.Any())
            {
                return (null, validationResultDto);
            }

            _unitOfWork.Categories.Delete(entity);

            await _unitOfWork.Complete();

            var dto = entity?.ConvertToDto();

            return (dto, validationResultDto);
        }
    }
}
