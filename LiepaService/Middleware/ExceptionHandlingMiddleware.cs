using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiepaService.Models.Views;
using LiepaService.Extensions;

namespace LiepaService.Middleware {
    
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ResponseView {
                Success = false,
                ErrorId = exception.HResult,
                ErrorMsg = exception.Message
            };

            await context.WriteResultAsync(new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
}