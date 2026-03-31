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
                await HandleExceptionAsync(context, ex);
            }
        }

        //it is for a middleware not the controller 
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Errors = (exception as ValidationException)?.Errors //dictionary only in the validation error 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
