namespace CrudDapperGraphQL.Middleware
{
    public class ExceptionHandlerMiddleware

    {
        /*
            private readonly RequestDelegate _next;

            public ExceptionHandlerMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new
                {
                    status = context.Response.StatusCode,
                    error = "An error occurred while processing your request.",
                    details = exception.Message
                });

                return context.Response.WriteAsync(result);
            }
        */
    }
}
