using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts;
using CrudDapperGraphQL.Data.Repositories;
using CrudDapperGraphQL.Services;
using Microsoft.Extensions.Configuration;

namespace CrudDapperGraphQL.AppStart
{
    public static class ServiceRegistration
    {
        public static void AddDependentServices(this IServiceCollection services)
        {

            services.AddSingleton<ApplicationDbContext>();

            services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();

            services.AddTransient<IBookService, BookService>();

            services.AddControllers();
        }
    }
}
