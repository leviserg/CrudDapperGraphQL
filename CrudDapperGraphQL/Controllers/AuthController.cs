using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CrudDapperGraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiAuthModel _apiAuthModel;
        private readonly IAuthService _authService;
        public AuthController(
            IOptions<ApiAuthModel> apiAuthModel,
            IAuthService authService)
        {
            this._apiAuthModel = apiAuthModel.Value;
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] InputAuthModel inpAuthModel)
        {
            ApiAuthModel? auth = (inpAuthModel?.ClientId != _apiAuthModel.ClientId) ? null : _apiAuthModel;

            if (auth == null)
                return BadRequest();

            if (!AuthExtensions.VerifyMd5Hash(inpAuthModel?.ClientSecret, auth.ClientSecretHash))
                return BadRequest();

            var token = await _authService.GetTokenAsync(auth);

            return Ok(token);
        }

    }
}
