using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data.Contracts.Services;

namespace CrudDapperGraphQL.AppStart
{
    public static class ApplicationSetup
    {
        public static async Task InitializeOptionsAsync(IServiceProvider serviceProvider)
        {
            //var _service = serviceProvider.GetRequiredService<IAuthorService>();
            AuthOptions.Initialize();

            await Task.CompletedTask;
        }
    }
}
