using Microsoft.AspNetCore.Builder;
using LiepaService.Middleware;

namespace LiepaService.Extensions {
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLiepaExceptionHandling(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}