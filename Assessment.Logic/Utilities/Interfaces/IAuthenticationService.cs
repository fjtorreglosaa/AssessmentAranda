using Assessment.Logic.Dtos.AuthenticationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Logic.Utilities.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponseDto> BuildToken(UserCredentialsDto userCredentials);
    }
}
