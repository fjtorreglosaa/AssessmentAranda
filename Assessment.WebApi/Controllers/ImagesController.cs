using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.ImageDtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.WebApi.Controllers
{
    [Route("api/v1/imagenes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagesController : ControllerBase
    {
        private readonly IImageDomainService _imageDomainService;

        public ImagesController(IImageDomainService imageDomainService)
        {
            _imageDomainService = imageDomainService;
        }

        /// <summary>
        /// Carga una imagen de un Producto
        /// </summary>
        /// <param name="critera">Ruta donde se encuntra la imagen a cargar</param>
        /// <returns></returns>
        [HttpPost(Name = "cargarImagen")]
        public async Task<IActionResult> UploadImage([FromBody] CreateImageDto critera)
        {
            var result = await _imageDomainService.UploadImageFromPathAsync(critera);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Retorna un listado con todas las imagenes
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpGet(Name = "obtenerImagenes")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _imageDomainService.GetAllImagesAsync();

            return Ok(result);
        }
    }
}
