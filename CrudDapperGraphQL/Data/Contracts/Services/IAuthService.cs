using CrudDapperGraphQL.Auth;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IAuthService
    {
        Task<TokenModel> GetTokenAsync(ApiAuthModel apiAuthModel);
    }
}
