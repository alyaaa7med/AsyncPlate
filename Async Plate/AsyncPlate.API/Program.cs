using AsyncPlate.API.Middlewares;
using AsyncPlate.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. DATABASE CONFIGURATION ---
// Hardcode the string here temporarily to ensure the API can always find it
var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AsyncPlateDb;Integrated Security=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. REGISTRATION AREA (Dependency Injection) ---
//transient -> per ordered 
//scoped -> per request 
//singletone -> per application 




var app = builder.Build();

// --- 3. MIDDLEWARE PIPELINE ---
app.UseMiddleware<ExceptionMiddleware>();
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