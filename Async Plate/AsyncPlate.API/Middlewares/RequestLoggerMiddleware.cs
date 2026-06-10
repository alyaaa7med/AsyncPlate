using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AsyncPlate.API.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggerMiddleware> _logger;

        public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        //RequestLogger → logs request, response time, status code for successful requests.

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "HTTP {Method} {Path} requested",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} completed in {Elapsed}ms. Status: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode);
        }
    }

}
