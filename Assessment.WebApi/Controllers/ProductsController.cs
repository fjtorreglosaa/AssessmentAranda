using Assessment.Data.Entities;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Dtos.PaginationDtos;
using Assessment.Logic.Dtos.ProductDtos;
using Assessment.Logic.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.WebApi.Controllers
{
    [Route("api/v1/productos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductDomainService _productDomainService;

        public ProductsController(IProductDomainService productDomainService)
        {
            _productDomainService = productDomainService;
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="criteria">Criterios del producto a crear</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPost(Name = "crearProducto")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto criteria)
        {
            var result = await _productDomainService.CreateProduct(criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Retorna una lista con todos los productos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(Name = "obtenerProductos")]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationDto paginationDto)
        {
            var queryable = await _productDomainService.GetProducts();
            await HttpContext.InsertPaginationParametersOnHeader(queryable);

            var products = queryable.OrderBy(product => product.Id).Paginate(paginationDto).ToList();

            var result = products.Select(p => p.ConvertToDto()).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Retorna una lista de productos de acuerdo al nombre proporcionado
        /// </summary>
        /// <param name="name">Nombre del producto</param>
        /// <param name="paginationDto"></param>
        /// <param name="sortByAscending">Ordenar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("nombre", Name = "obtenerProductosPorNombre")]
        public async Task<IActionResult> GetProductsByName(string name, [FromQuery] PaginationDto paginationDto, bool sortByAscending = true)
        {
            var result = new List<ProductDto>();
            var category = await _productDomainService.GetProductsByName(name);
            var queryable = await _productDomainService.GetProductsByName(name);
            await HttpContext.InsertPaginationParametersOnHeader(queryable);

            var products = queryable.OrderBy(product => product.Name).Paginate(paginationDto).ToList();

            if (!sortByAscending)
            {
                result = products.OrderByDescending(p => p.Name).Select(p => p.ConvertToDto()).ToList();
                return Ok(result);
            }

            result = products.OrderBy(p => p.Name).Select(p => p.ConvertToDto()).ToList();
            return Ok(products);
        }

        /// <summary>
        /// Retorna una lista de productos de acuerdo a la categoria proporcionada
        /// </summary>
        /// <param name="category">Categoría del producto</param>
        /// <param name="paginationDto"></param>
        /// <param name="sortByAscending">Ordenar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("categoria", Name = "obtenerProductosPorCategoria")]
        public async Task<IActionResult> GetProductsByCategory(string category, [FromQuery] PaginationDto paginationDto, bool sortByAscending = true)
        {
            var result = new List<ProductDto>();
            var queryable = await _productDomainService.GetProductsByCategory(category);

            if (queryable.ValidationResult.Conditions.Any())
            {
                return BadRequest(queryable.ValidationResult);
            }

            await HttpContext.InsertPaginationParametersOnHeader(queryable.Queryable);

            var products = queryable.Queryable.Paginate(paginationDto).ToList();

            if (!sortByAscending)
            {
                result = products.OrderByDescending(p => p.Name).Select(p => p.ConvertToDto()).ToList();
                return Ok(result);
            }

            result = products.OrderBy(p => p.Name).Select(p => p.ConvertToDto()).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Retorna una lista de productos de acuerdo a la descripción proporcionada
        /// </summary>
        /// <param name="description">Descripción del producto</param>
        /// <param name="paginationDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("descripcion", Name = "obtenerProductosPorDescripcion")]
        public async Task<IActionResult> GetProductsByDescription(string description, [FromQuery] PaginationDto paginationDto)
        {
            var queryable = await _productDomainService.GetProductsByDescription(description);
            await HttpContext.InsertPaginationParametersOnHeader(queryable);

            var products = queryable.OrderBy(product => product.Name).Paginate(paginationDto).ToList();

            var result = products.Select(p => p.ConvertToDto()).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Retorna el producto con el id proporcionado
        /// </summary>
        /// <param name="id">Id del producto buscado</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "obtenerProducto")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productDomainService.GetProductById(id);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Modifica un producto de acuerdo al id proporcionado
        /// </summary>
        /// <param name="id">Id del producto a actualizar</param>
        /// <param name="criteria">Criterios de actualización</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPut("{id:int}", Name = "modificarProducto")]
        public async Task<IActionResult> UpdateProductById(int id, [FromBody] UpdateProductDto criteria)
        {
            var result = await _productDomainService.UpdateProductById(id, criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }

        /// <summary>
        /// Elimina el producto de acuerdo al id proporcionado
        /// </summary>
        /// <param name="id">Id del producto a eliminar</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpDelete("{id:int}", Name = "removerProducto")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productDomainService.DeleteProductById(id);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }

            return Ok(result.dto);
        }
    }
}
