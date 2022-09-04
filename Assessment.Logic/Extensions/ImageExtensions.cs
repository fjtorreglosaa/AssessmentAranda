using Assessment.Data.Entities;
using Assessment.Logic.Dtos.ImageDtos;

namespace Assessment.Logic.Extensions
{
    public static class ImageExtensions
    {
        public static Image ConvertToEntity(this CreateImageDto dto)
        {
            return new Image
            {
                Name = dto.Name
            };
        }

        public static ImageDto ConvertToDto(this Image entity)
        {
            return new ImageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ImagePath = entity.ImagePath,
                ImageType = entity.ImageType
            };
        }
    }
}
