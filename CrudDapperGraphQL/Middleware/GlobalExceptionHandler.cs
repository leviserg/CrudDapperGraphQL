using System.Net;
using System.Text.Json;
using System.Web.Http.ExceptionHandling;

namespace CrudDapperGraphQL.Middleware
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override bool ShouldHandle(ExceptionHandlerContext context) {
            if (context.RequestContext.Configuration.Properties.ContainsKey("MS_CorsEnabledKey")) {
                return (bool)context.RequestContext.Configuration.Properties["MS_CorsEnabledKey"]; 
            } return base.ShouldHandle(context); 
        }
    }
}
