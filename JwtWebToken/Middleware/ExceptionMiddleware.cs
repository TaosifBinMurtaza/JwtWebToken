using JwtWebToken.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace JwtWebToken.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                return _next(httpContext);
            }
            catch(Exception ex)
            {
                ApiError response;
                HttpStatusCode httpStatusCode=HttpStatusCode.InternalServerError;

                string message;
                var exceptiopnType = ex.GetType();


                if (exceptiopnType == typeof(UnauthorizedAccessException))
                {
                    httpStatusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorize.";
                }
                else
                {
                    message = "Something went wrong.";
                }
                if (_env.IsDevelopment())
                {
                    response = new ApiError((long)httpStatusCode, ex.Message, ex.StackTrace);
                }
                else
                {
                    response = new ApiError((long)httpStatusCode, message);
                }

                httpContext.Response.StatusCode = (int)httpStatusCode;
                httpContext.Response.ContentType = "application/json";
                return httpContext.Response.WriteAsync(response.ToJson());
               
            }

          
        }
    }

    //Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
