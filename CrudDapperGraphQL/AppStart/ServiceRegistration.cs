using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using CrudDapperGraphQL.Data.Repositories;
using CrudDapperGraphQL.GraphQL;
using CrudDapperGraphQL.Health;
using CrudDapperGraphQL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CrudDapperGraphQL.AppStart
{
    public static class ServiceRegistration
    {
        public static void AddDependentServices(this IServiceCollection services)
        {

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("Database");
            // AspNetCore.HealthChecks.Mysql;SqlServer;MongoDb;Kafka;Network;CosmosDb;Oracleand lot more NuGet packages available for use cases
            // E.G. - for PostgreSql: .AddNpgSql(config["Database:ConnectionString"]!)
            // .AddCheck<AnotherHealthCheck>("AnotherService") // add additional service health check here which implements IHealthCheck interface

            services.AddSingleton<ApplicationDbContext>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddScoped<IRepository<Author, AuthorSave>, AuthorRepository>();
            services.AddScoped<IRepository<Book, BookSave>, BookRepository>();

            services.AddTransient<IEntityService<Author, AuthorSave>, AuthorService>();
            services.AddTransient<IEntityService<Book, BookSave>, BookService>();

            services.AddControllers();

            // Authorization & Authentication

            var parameters = new TokenValidationParameters();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        RequireAudience = true,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        IssuerSigningKey = AuthOptions.PublicKey,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["token"];

                            if (!string.IsNullOrWhiteSpace(accessToken) &&
                                context.Request.Path.StartsWithSegments("/api"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // Swagger

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {

                // Add security definition for Bearer token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add a global security requirement for Bearer token
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            // GraphQL
            services.AddGraphQLServer()
                .RegisterService<IEntityService<Book, BookSave>>()
                .RegisterService<IEntityService<Author, AuthorSave>>()
                .AddAuthorization()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddErrorFilter<AuthorizationErrorFilter>();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = 307;
                options.HttpsPort = 8081;
            });

        }

        public static void AddGlobalErrorHandling(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                };
            });
        }
    }
}
