using CisApi.Presentation.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebApplicationBuilderExtension
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
