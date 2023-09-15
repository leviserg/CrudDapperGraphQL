using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.GraphQL;
using CrudDapperGraphQL.Data.Repositories;
using CrudDapperGraphQL.Services;
using Microsoft.Extensions.Configuration;
using HotChocolate;

namespace CrudDapperGraphQL.AppStart
{
    public static class ServiceRegistration
    {
        public static void AddDependentServices(this IServiceCollection services)
        {

            services.AddSingleton<ApplicationDbContext>();

            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IBookService, BookService>();

            services.AddControllers();

            // GraphQL
            services.AddHttpClient<BookRepository>();
            services.AddHttpClient<BookService>();
            services.AddGraphQLServer().AddQueryType<Query>();
            
        }
    }
}
