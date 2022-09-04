using Assessment.Logic.Dtos.ImageDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Logic.DomainServices.Interfaces
{
    public interface IImageDomainService
    {
        Task<(ImageDto dto, ValidationResultDto ValidationResult)> UploadImageFromPathAsync(CreateImageDto criteria);
        Task<List<ImageDto>> GetAllImagesAsync();
    }
}
