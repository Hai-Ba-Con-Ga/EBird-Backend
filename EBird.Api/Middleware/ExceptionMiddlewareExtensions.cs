using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using static AutoWrapper.Models.ApiProblemDetailsExceptionResponse;
using System.Net;

namespace EBird.Api.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        Console.WriteLine(contextFeature.Error);
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        //await context.Response.WriteAsync();
                    }
                });
            });
        }
    }
}
