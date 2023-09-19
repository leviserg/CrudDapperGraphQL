namespace CrudDapperGraphQL.Auth
{
    public class AuthorizationErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            
            if (error.Extensions.ContainsKey("code") && error.Extensions["code"].Equals("AUTH_NOT_AUTHORIZED"))
            {
                var handledError = new Error("You currently not authorized to access selected resource",
                    "401",
                    null,
                    null,
                    null, 
                    new UnauthorizedAccessException());
                return handledError;
            }

            return error;
        }
    }
}
