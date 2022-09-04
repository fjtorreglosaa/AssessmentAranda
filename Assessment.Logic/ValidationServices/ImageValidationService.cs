using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Logic.Dtos.ImageDtos;
using Assessment.Logic.Dtos.ValidationDtos;
using Assessment.Logic.ValidationServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Assessment.Logic.ValidationServices
{
    public class ImageValidationService : IImageValidationService
    {
        private readonly IEFUnitOfWork _unitOfWork;

        public ImageValidationService(IEFUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidationResultDto> ValidateForCreate(CreateImageDto criteria)
        {
            var validationResultDto = new ValidationResultDto();
            var imageLocation = $"{criteria.ImagePath}";
            var images = await _unitOfWork.Images.GetAll().Where(i => i.Name == criteria.Name).ToListAsync();

            if (images.Any())
            {
                validationResultDto.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"La imagen con nombre {criteria.Name} ya existe.",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (string.IsNullOrEmpty(criteria.ImagePath))
            {
                validationResultDto.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"Se debe proporcionar una ruta para la imagen",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (string.IsNullOrEmpty(criteria.Name))
            {
                validationResultDto.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"Se debe proporcionar una nombre para la imagen",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            if (!File.Exists(imageLocation))
            {
                validationResultDto.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = $"El archivo proporcionado no existe en la ruta: {criteria.ImagePath}",
                    Severity = (int)HttpStatusCode.NotFound
                });
            }

            return validationResultDto;
        }
    }
}
