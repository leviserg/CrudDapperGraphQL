using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Repositories;
using CrudDapperGraphQL.GraphQL;
using CrudDapperGraphQL.Services;

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
            services.AddGraphQLServer()
                .RegisterService<IBookService>()
                .RegisterService<IAuthorService>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();

        }
    }
}
