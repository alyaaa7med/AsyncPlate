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



        public async Task InvokeAsync(HttpContext context)
        {

            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("HTTP {Method} {Path} requested",
            context.Request.Method, context.Request.Path);

                await _next(context);
            }
            finally // This runs even if a crash happens! ( exmiddleware: try (loggmid: try   finally)  catch)
            {
                stopwatch.Stop();
                _logger.LogInformation(
                        "Request {Method} {Path} took {Elapsed}ms. Status: {StatusCode}",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.ElapsedMilliseconds,
                        context.Response.StatusCode);
            }
        }
    }

}
