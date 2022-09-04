using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.CategoryDtos;
using Assessment.Logic.Dtos.PaginationDtos;
using Assessment.Logic.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.WebApi.Controllers
{
    [Route("api/v1/categorias")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryDomainService _categoryDomainService;

        public CategoriesController(ICategoryDomainService categoryDomainService)
        {
            _categoryDomainService = categoryDomainService;
        }

        /// <summary>
        /// Crea una categoria nueva
        /// </summary>
        /// <param name="criteria">Nombre de la categoria a crear</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPost(Name = "crearCategoria")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto criteria)
        {
            var result = await _categoryDomainService.CreateCategory(criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Retorna una lista de todas las categorias
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(Name = "obtenerCategorias")]
        public async Task<IActionResult> GetAllCategories([FromQuery] PaginationDto paginationDto)
        {
            var queryable = await _categoryDomainService.GetAllAsync();
            await HttpContext.InsertPaginationParametersOnHeader(queryable);

            var categories = queryable.OrderBy(product => product.Name).Paginate(paginationDto).ToList();

            var result = categories.Select(r => r.ConvertToDto()).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el elemento con id proporcionado
        /// </summary>
        /// <param name="id">Id de la categoria a buscar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "obtenerCategoria")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryDomainService.GetCategoryByIdAsync(id);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Actualiza el nombre de la categoria con el id proporcionado
        /// </summary>
        /// <param name="id">Id de la categoria a actualizar</param>
        /// <param name="criteria">Criterios de actualización</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPut("{id:int}", Name = "modificarCategoria")]
        public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] UpdateCategoryDto criteria)
        {
            var result = await _categoryDomainService.UpdateCategoryByIdAsync(id, criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Elimina la categoria con el id proporcionado
        /// </summary>
        /// <param name="id">Id de la categoria a eliminar</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpDelete("{id:int}", Name = "removerCategoria")]
        public async Task<IActionResult> DeleteCategoryByID(int id)
        {
            var result = await _categoryDomainService.DeleteCategoryById(id);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }
    }
}
