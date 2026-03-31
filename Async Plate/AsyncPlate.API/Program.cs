using AsyncPlate.API.Middlewares;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// --- 1. REGISTRATION AREA (Dependency Injection) ---
builder.Services.AddTransient<RequestLoggerMiddleware>(); //one per injection
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); //use & first one as it is the whole wrapper 
app.UseMiddleware<RequestLoggerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
