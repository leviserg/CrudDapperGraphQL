using CrudDapperGraphQL.Auth;

namespace CrudDapperGraphQL.Data.Contracts.Services
{
    public interface IAuthConfigurationService
    {
        Task<SecretKeysModel> GetSecretKeys();
    }
}
