using Assessment.Logic.Dtos.AuthenticationDtos;
using Assessment.Logic.Dtos.PermissionDtos;
using Assessment.Logic.Utilities.Interfaces;
using Assessment.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assessment.WebApi.Controllers
{
    [Route("api/v1/cuentas")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [HttpPost("registro", Name = "registro")]
        public async Task<ActionResult<AuthenticationResponseDto>> Register(UserCredentialsDto userCredentials)
        {
            var user = new IdentityUser
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email
            };

            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                var token = await _authenticationService.BuildToken(userCredentials);

                return token;
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Logearse con un usuario existente
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [HttpPost("sesion", Name = "login")]
        [ServiceFilter(typeof(ActionLogFilter))]
        public async Task<ActionResult<AuthenticationResponseDto>> Login(UserCredentialsDto userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await _authenticationService.BuildToken(userCredentials);
            }

            return BadRequest("Se ha producido un error al intentar logearse.");
        }

        /// <summary>
        /// Renueva el token de autenticación
        /// </summary>
        /// <returns></returns>
        [HttpGet("credenciales", Name = "renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponseDto>> RenewToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim?.Value;

            var userCredentials = new UserCredentialsDto
            {
                Email = email,
            };

            return await _authenticationService.BuildToken(userCredentials);
        }

        /// <summary>
        /// Otorga permisos de administrador a un usuario
        /// </summary>
        /// <param name="editAdminDto"></param>
        /// <returns></returns>
        [HttpPost("permisos", Name = "hacerAdministrador")]
        public async Task<ActionResult> CreateAdminPermissions(EditAdminDto editAdminDto)
        {
            var user = await _userManager.FindByEmailAsync(editAdminDto.Email);

            await _userManager.AddClaimAsync(user, new Claim("admin", "1"));

            return NoContent();
        }

        /// <summary>
        /// Remueve los permisos de administrador a un usuario
        /// </summary>
        /// <param name="editAdminDto"></param>
        /// <returns></returns>
        [HttpDelete("permisos", Name = "removerAdministrador")]
        public async Task<ActionResult> RemoveAdminPermissions(EditAdminDto editAdminDto)
        {
            var user = await _userManager.FindByEmailAsync(editAdminDto.Email);

            await _userManager.RemoveClaimAsync(user, new Claim("admin", "1"));

            return NoContent();
        }
    }   
}
