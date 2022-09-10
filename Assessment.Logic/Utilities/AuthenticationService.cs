using Assessment.Logic.Dtos.AuthenticationDtos;
using Assessment.Logic.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assessment.Logic.Utilities
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<AuthenticationResponseDto> BuildToken(UserCredentialsDto userCredentials)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimsInDb = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsInDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            var authenticationResponse = new AuthenticationResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };

            return authenticationResponse;
        }

    }
}
