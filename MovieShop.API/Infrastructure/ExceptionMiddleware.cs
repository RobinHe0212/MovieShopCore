using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieShop.Core.Exceptions;
using Newtonsoft.Json;

namespace MovieShop.API.Infrastructure
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }

           
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            string result;
            if (env.IsDevelopment())
            {
                var errorDetail = new ErrorResponseModel
                {
                    ErrorMessgae = ex.Message,
                    ExceptionStackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.Message
                };
                result = JsonConvert.SerializeObject(new { error = errorDetail });

            }
            else
            {
                result = JsonConvert.SerializeObject(new { error = ex.Message });
            }


            switch (ex)
            {
                case UnauthorizedAccessException _:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case DivideByZeroException _:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case Exception _:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    break;
            }
           await httpContext.Response.WriteAsync(result);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
