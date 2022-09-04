using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.ImageDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.Extensions;
using Assessment.Logic.Utilities;
using Assessment.Logic.ValidationServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Logic.DomainServices
{
    public class ImageDomainService : IImageDomainService
    {
        private readonly IEFUnitOfWork _unitOfWork;
        private readonly IImageValidationService _imageValidationService;

        public ImageDomainService(IEFUnitOfWork unitOfWork, IImageValidationService imageValidationService)
        {
            _unitOfWork = unitOfWork;
            _imageValidationService = imageValidationService;
        }

        public async Task<(ImageDto dto, ValidationResultDto ValidationResult)> UploadImageFromPathAsync(CreateImageDto criteria)
        {
            var validationResult = await _imageValidationService.ValidateForCreate(criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var currentImageName = criteria.ImagePath.Split("\\").LastOrDefault();
            var sourcePath = @$"{criteria.ImagePath.Substring(0, criteria.ImagePath.Length - currentImageName.Length - 1)}";
            var targetPath = @$"{Directory.GetCurrentDirectory()}{StringConstants.IMG_FOLDER}";
            var fileName = $"{criteria.Name}{StringConstants.JPG}";
            var entity = criteria.ConvertToEntity();
            entity.ImagePath = @$"\{StringConstants.JPG}\{fileName}";
            entity.ImageType = StringConstants.PRODUCT;

            ManageFileService.CopyFileToLocation(currentImageName, fileName, sourcePath, targetPath);

            await _unitOfWork.Images.Add(entity);
            await _unitOfWork.Complete();

            var dto = entity.ConvertToDto();

            return (dto, validationResult);
        }

        public async Task<List<ImageDto>> GetAllImagesAsync()
        {
            var entities = await _unitOfWork.Images.GetAll().ToListAsync();

            var dtos = entities.Select(i => i.ConvertToDto()).ToList();

            return dtos;
        }
    }
}
