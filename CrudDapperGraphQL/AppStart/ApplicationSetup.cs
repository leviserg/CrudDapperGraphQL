using CrudDapperGraphQL.Auth;
using Microsoft.AspNetCore.Diagnostics;

namespace CrudDapperGraphQL.AppStart
{
    public static class ApplicationSetup
    {
        public static WebApplication InitializeAuthOptions(this WebApplication app)
        {
            AuthOptions.Initialize();
            return app;
        }

        public static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseExceptionHandler("/error");

            app.Map("/error", 
                (HttpContext httpContext) => {
                    Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

                    if (exception is null) {
                        // handle unexpected case => logging
                        return Results.Problem();
                    }

                    // custom global error handling logic

                    return Results.Problem();

                }
            );

            return app;
        }
    }
}
