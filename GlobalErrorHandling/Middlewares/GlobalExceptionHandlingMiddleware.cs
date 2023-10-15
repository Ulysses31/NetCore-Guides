using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GlobalErrorHandling.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger
        ) => _logger = logger;

        public async Task InvokeAsync(
            HttpContext context,
            RequestDelegate next
        )
        {
            _logger.LogInformation("--> GlobalExceptionHandlingMiddleware invoking...");

            try
            {
                await next(context);

                // string bodyContent =
                //     await new StreamReader(
                //         context.Response.Body
                //     ).ReadToEndAsync();

                // _logger.LogInformation($"--> Response: {bodyContent}");
            }
            catch (System.Exception e)
            {
                await BuildException(context, e);
            }
        }

        private async Task BuildException(HttpContext context, Exception e)
        {
            _logger.LogError(e, $"--> {e.Message}");

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            ProblemDetails problem = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server Error",
                Title = "Internal Server Error",
                Detail = e.Message,
                Instance = $"{e.TargetSite?.DeclaringType?.FullName} - Method: [{e.TargetSite?.Name}]"
            };

            var json = JsonSerializer.Serialize<ProblemDetails>(problem);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
