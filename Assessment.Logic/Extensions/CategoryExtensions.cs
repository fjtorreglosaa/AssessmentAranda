using Assessment.Data.Entities;
using Assessment.Logic.Dtos.CategoryDtos;

namespace Assessment.Logic.Extensions
{
    public static class CategoryExtensions
    {
        public static Category ConvertToEntity(this CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
            };
        }

        public static CategoryDto ConvertToDto(this Category entity)
        {
            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
