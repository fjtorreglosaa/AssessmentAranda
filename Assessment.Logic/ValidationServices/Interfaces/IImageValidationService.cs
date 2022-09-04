using Assessment.Logic.Dtos.ImageDtos;
using Assessment.Logic.Dtos.ValidationDtos;

namespace Assessment.Logic.ValidationServices.Interfaces
{
    public interface IImageValidationService
    {
        Task<ValidationResultDto> ValidateForCreate(CreateImageDto criteria);
    }
}
