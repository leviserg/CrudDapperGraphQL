using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data.Contracts.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CrudDapperGraphQL.Services
{
    public class AuthService : IAuthService
    {
        private TokenModel _cachedToken;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<TokenModel> GetTokenAsync(ApiAuthModel apiAuthModel)
        {
            if (_cachedToken != null 
                && (DateTime.UtcNow - _cachedToken?.CreatedAt)?.TotalSeconds < _cachedToken.ExpirationSeconds)
            {
                return _cachedToken;
            }

            await _semaphore.WaitAsync();
            try
            {
                if (_cachedToken == null 
                    || (DateTime.UtcNow - _cachedToken?.CreatedAt)?.TotalSeconds >= _cachedToken.ExpirationSeconds)
                {
                    _cachedToken = GenerateJwt(apiAuthModel);
                    _cachedToken.CreatedAt = DateTime.UtcNow;
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return _cachedToken;
        }

        private TokenModel GenerateJwt(ApiAuthModel apiAuthModel)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: GetClaims(apiAuthModel),
                expires: now.AddMinutes(AuthOptions.LifeTimeMinutes),
                signingCredentials: new SigningCredentials(AuthOptions.PrivateKey, SecurityAlgorithms.RsaSha256)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenModel { Token = tokenString, ExpirationSeconds = AuthOptions.LifeTimeMinutes * 60 };
        }

        private IEnumerable<Claim> GetClaims(ApiAuthModel apiAuthModel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, apiAuthModel.ClientId),
                new Claim(ClaimTypes.NameIdentifier, apiAuthModel.ClientId)
            };

            return claims;
        }
    }
}
