using AsyncPlate.Core.Exceptions;
using System.ComponentModel;
using System.Net;
using System.Text.Json;

namespace AsyncPlate.API.Middlewares
{
    public class ExceptionMiddleware
    {
        //it is about the http request not specific action-> middleware (before + invoke + after)
        //i need to add middleware for catching execptions -> method contains (try + catch )

        private readonly RequestDelegate _next; //point to a function of this signature 
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //before 
                await _next(context); //call the next middleware 
            }
            catch (Exception ex)
            {
                //after returning
                _logger.LogError("An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        //it is for a middleware not the controller 
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                BadRequestException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;         ///for http not for the response body


            var response = new
            {
                IsSuccess = false,
                Message = exception.Message,
                Errors = (exception as ValidationException)?.Errors
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);

        }
    }
}
