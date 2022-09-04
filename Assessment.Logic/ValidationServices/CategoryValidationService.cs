using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Data.Entities;
using Assessment.Logic.Dtos.CategoryDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.ValidationServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Assessment.Logic.ValidationServices
{
    public class CategoryValidationService : ICategoryValidationService
    {
        private readonly IEFUnitOfWork _unitOfWork;

        public CategoryValidationService(IEFUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(Category Category, ValidationResultDto ValidationResult)> ValidateForName(string criteria)
        {
            var validationResult = new ValidationResultDto();
            var category = await _unitOfWork.Categories.GetAll().FirstOrDefaultAsync(c => c.Name == criteria);

            if (category == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"La categoria con el nombre {criteria} no existe",
                    Severity = (int)HttpStatusCode.NotFound
                });

                return (null, validationResult);
            }

            return (category, validationResult);
        }

        public async Task<ValidationResultDto> ValidateForCreate(CreateCategoryDto criteria)
        {
            var validationResult = new ValidationResultDto();
            var categories = await _unitOfWork.Categories.GetAll().Where(c => c.Name == criteria.Name).ToListAsync();

            if (string.IsNullOrEmpty(criteria.Name))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "El nombre de la categoría es obligatorio.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (categories.Any())
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El nombre {criteria.Name} de la categoría ya existe.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (criteria.Name.Length > 50)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El nombre {criteria.Name} excede los 50 caracteres permitidos.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            return validationResult;
        }

        public async Task<ValidationResultDto> ValidateForGet(int id, Category entity)
        {
            var validationResult = new ValidationResultDto();

            if (entity == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"No existe una categoría con id {id}",
                    Severity = (int)HttpStatusCode.NotFound
                });
            }

            return validationResult;
        }

        public async Task<ValidationResultDto> ValidateForUpdate(int id, UpdateCategoryDto criteria)
        {
            var validationResult = new ValidationResultDto();

            if (string.IsNullOrEmpty(criteria?.Name))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "El nombre de la categoria no puede se vacio.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (criteria?.Name.Length > 50)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El nombre {criteria?.Name} excede los 50 caracteres permitidos.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            return validationResult;
        }
    }
}
