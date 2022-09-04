using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Data.Entities;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.ProductDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.ValidationServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Assessment.Logic.ValidationServices
{
    public class ProductValidationService : IProductValidationService
    {
        private readonly IEFUnitOfWork _unitOfWork;
        private readonly IImageDomainService _imageDomainService;

        public ProductValidationService(IEFUnitOfWork unitOfWork, IImageDomainService imageDomainService)
        {
            _unitOfWork = unitOfWork;
            _imageDomainService = imageDomainService;
        }

        public async Task<ValidationResultDto> ValidateForCreate(CreateProductDto criteria)
        {
            var validationResult = new ValidationResultDto();
            var Products = await _unitOfWork.Products.GetAll().Where(c => c.Name == criteria.Name).ToListAsync();
            var category = await _unitOfWork.Categories.GetAll().Where(i => i.Id == criteria.CategoryId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(criteria.Name))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "El nombre del producto es obligatorio.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (Products.Any())
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El nombre {criteria.Name} del producto ya existe.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (category == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"No existe una categoria con id {criteria.CategoryId}.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (criteria.Image != null)
            {
                var validateImage = await _imageDomainService.UploadImageFromPathAsync(criteria.Image);

                if (validateImage.ValidationResult.Conditions.Any())
                {
                    validationResult.Conditions.AddRange(validateImage.ValidationResult.Conditions);
                }
            }

            if (criteria.Name.Length > 50)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El nombre {criteria.Name} excede los 50 caracteres permitidos.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (criteria.Price < 0)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El precio no puede ser inferior a cero.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            return validationResult;
        }
        public async Task<ValidationResultDto> ValidateForGet(int id, Product entity)
        {
            var validationResult = new ValidationResultDto();

            if (entity == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"No existe un producto con id {id}",
                    Severity = (int)HttpStatusCode.NotFound
                });
            }

            return validationResult;
        }

        public async Task<ValidationResultDto> ValidateForUpdate(int id, UpdateProductDto criteria)
        {
            var validationResult = new ValidationResultDto();
            var category = await _unitOfWork.Categories.GetAll().Where(i => i.Id == criteria.CategoryId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(criteria?.Name))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "El nombre del producto no puede se vacio.",
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

            if (category == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"La categoría con id {criteria.CategoryId} no existe.",
                    Severity = (int)HttpStatusCode.NotFound
                });
            }

            return validationResult;
        }
    }
}
