using Assessment.Data.Entities;
using Assessment.Logic.Dtos.ProductDtos;

namespace Assessment.Logic.Extensions
{
    public static class ProductExtensions
    {
        public static Product ConvertToEntity(this CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Image = dto.Image?.ConvertToEntity()
            };
        }

        public static ProductDto ConvertToDto(this Product entity)
        {
            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                CategoryId = entity.CategoryId,
                ImageId = entity.ImageId
            };
        }
    }
}
