using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data.Contracts.Services;

namespace CrudDapperGraphQL.AppStart
{
    public static class ApplicationSetup
    {
        public static async Task InitializeAuthOptionsAsync(IServiceProvider serviceProvider)
        {
            var authConfigurationService = serviceProvider.GetRequiredService<IAuthConfigurationService>();
            var secretKeysModel = await authConfigurationService.GetSecretKeys();
            AuthOptions.Initialize(secretKeysModel);
        }
    }
}
