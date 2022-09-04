using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Data.Entities;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.ProductDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.Extensions;
using Assessment.Logic.ValidationServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Logic.DomainServices
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IEFUnitOfWork _unitOfWork;
        private readonly IProductValidationService _productValidationService;
        private readonly ICategoryValidationService _categoryValidationService;

        public ProductDomainService(IEFUnitOfWork unitOfWork, IProductValidationService productValidationService, ICategoryValidationService categoryValidationService)
        {
            _unitOfWork = unitOfWork;
            _productValidationService = productValidationService;
            _categoryValidationService = categoryValidationService;
        }

        public async Task<(ProductDto dto, ValidationResultDto ValidationResult)> CreateProduct(CreateProductDto criteria)
        {
            var validationResult = await _productValidationService.ValidateForCreate(criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var entity = criteria.ConvertToEntity();

            if (criteria.Image != null)
            {
                var image = await _unitOfWork.Images.GetAll().FirstOrDefaultAsync(img => img.Name == criteria.Image.Name);
                if (image != null) entity.ImageId = image.Id;
                entity.Image = image;
            }

            await _unitOfWork.Products.Add(entity);
            await _unitOfWork.Complete();

            var dto = entity.ConvertToDto();

            return (dto, validationResult);

        }

        public async Task<IQueryable<Product>> GetProducts()
        {
            var queryable = _unitOfWork.Products.GetAll();

            return queryable;
        }

        public async Task<IQueryable<Product>> GetProductsByName(string criteria)
        {
            var products = await _unitOfWork.Products.GetProductsByNameAsync(criteria);

            return products;
        }

        public async Task<IQueryable<Product>> GetProductsByDescription(string criteria)
        {
            var products = await _unitOfWork.Products.GetProductsByNameAsync(criteria);

            return products;
        }

        public async Task<(IQueryable<Product> Queryable, ValidationResultDto ValidationResult)> GetProductsByCategory(string criteria)
        {
            var validationResult = await _categoryValidationService.ValidateForName(criteria);

            if (validationResult.ValidationResult.Conditions.Any())
            {
                return (null, validationResult.ValidationResult);
            }

            var products = await _unitOfWork.Products.GetProductsByCategoryAsync(validationResult.Category.Id);

            await products.ForEachAsync(p => p.Category = validationResult.Category);

            return (products, validationResult.ValidationResult);
        }

        public async Task<(ProductDto dto, ValidationResultDto ValidationResult)> GetProductById(int id)
        {
            var entity = await _unitOfWork.Products.GetByIdAsync(id);

            var validationResult = await _productValidationService.ValidateForGet(id, entity);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<(ProductDto dto, ValidationResultDto ValidationResult)> UpdateProductById(int id, UpdateProductDto criteria)
        {
            var entity = await _unitOfWork.Products.GetByIdAsync(id);

            var validationResult = await _productValidationService.ValidateForGet(id, entity);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            validationResult = await _productValidationService.ValidateForUpdate(id, criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            entity.Name = criteria.Name;
            entity.Description = criteria.Description;
            entity.Price = criteria.Price;

            await _unitOfWork.Complete();

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<(ProductDto dto, ValidationResultDto ValidationResult)> DeleteProductById(int id)
        {
            var entity = await _unitOfWork.Products.GetByIdAsync(id);

            var validationResultDto = await _productValidationService.ValidateForGet(id, entity);

            if (validationResultDto.Conditions.Any())
            {
                return (null, validationResultDto);
            }

            _unitOfWork.Products.Delete(entity);

            await _unitOfWork.Complete();

            if (entity.ImageId != null)
            {
                var imageToDelete = await _unitOfWork.Images.GetAll().FirstOrDefaultAsync(img => img.Id == id);

                if (imageToDelete != null)
                {
                    _unitOfWork.Images.Delete(imageToDelete);

                    await _unitOfWork.Complete();
                }
            }

            var dto = entity?.ConvertToDto();

            return (dto, validationResultDto);
        }
    }
}
