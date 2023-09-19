using CrudDapperGraphQL.Auth;
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
        private readonly ApiUser _apiUser;
        public AuthController(IOptions<ApiUser> apiUser)
        {
            this._apiUser = apiUser.Value;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] AuthModel authModel)
        {
            ApiUser? user = (authModel?.Login != _apiUser.Login) ? null : _apiUser;

            if (user == null)
                return BadRequest();

            if (user.PasswordHash != authModel?.Password.GetMd5Hash()) //GetSha1()//
                return BadRequest();

            var token = GetJwt(user);

            return Ok(token);
        }

        private TokenModel GetJwt(ApiUser apiUser)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: GetClaims(apiUser),
                expires: now.AddMinutes(AuthOptions.LifeTimeMinutes),
                signingCredentials: new SigningCredentials(AuthOptions.PrivateKey, SecurityAlgorithms.RsaSha256)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenModel { Token = tokenString, ExpirationSeconds = AuthOptions.LifeTimeMinutes * 60 };
        }

        private IEnumerable<Claim> GetClaims(ApiUser apiUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, apiUser.Login),
                new Claim(ClaimTypes.NameIdentifier, apiUser.Login)
            };

            return claims;
        }
    }
}
