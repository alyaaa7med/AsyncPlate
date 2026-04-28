using AsyncPlate.API.Middlewares;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Mapping.Authentication;
using AsyncPlate.Core.Services.Implementation;
using AsyncPlate.Core.Services.Interfaces;
using AsyncPlate.Core.Validators.Authentication;
using AsyncPlate.Infrastructure;
using AsyncPlate.Infrastructure.Repository;
using AsyncPlate.Infrastructure.Services;
using AsyncPlate.Infrastructure.UnitOfWork;
using FluentValidation;
using Mailtrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. DATABASE & REPOSITORIES (Infrastructure)
// ==========================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICustomerRepo, GuestRepo>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ==========================================
// 2. VALIDATION & BUSINESS LOGIC
// ==========================================

builder.Services.AddScoped<IValidator<SignupCustomerRequestDTO>, SignupGuestRequestValidator>();
builder.Services.AddScoped<IValidator<SignupKitchenChefRequestDTO>, SignupKitchenChefRequestValidator>();
//builder.Services.AddScoped<IValidator<SignupCashierRequestDTO>, SignupCashierRequestValidator>();

// ==========================================
// 3. THIRD-PARTY SERVICES (Email & Mapping)
// ==========================================

builder.Services.AddTransient<IEmailService, MailTrapEmailService>();

builder.Services.Configure<MailtarpMappingClass>(builder.Configuration.GetSection("Mailtrap"));//for option pattern 

builder.Services.AddMailtrapClient(options =>
{
    var apiToken = builder.Configuration.GetSection("Mailtrap")["ApiToken"];
    if (string.IsNullOrWhiteSpace(apiToken))
    {
        throw new InvalidOperationException("Mailtrap ApiToken configuration is missing or empty.");
    }
    options.ApiToken = apiToken;
});

builder.Services.AddAutoMapper(cfg =>
{
    // This tells AutoMapper NOT to look at methods when mapping
    // which prevents it from hitting the MaxFloat LINQ bug.
    cfg.ShouldMapMethod = (m) => false;

    // Add your profiles here
    cfg.AddProfile<CustomerProfile>();
}, typeof(Program));

// ==========================================
// 4. API & SWAGGER CONFIGURATION
// ==========================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ==========================================
// 5. IDENTITY & JWT SECURITY
// ==========================================

//identity settings 
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//jwt settings (how the API will validate incoming tokens and what parameters to check)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// ==========================================
// 6. APP BUILDING & DATA SEEDING
// ==========================================

var app = builder.Build();

// --- ROLE SEEDING ADDED HERE ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "Admin", "Guest", "KitchenChef", "Cashier", "User" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles.");
    }
    //to dispose dbcontext , role manager objects to keep memory clean and avoid memory leaks
}

// ==========================================
// 7. MIDDLEWARE PIPELINE (The Assembly Line)
// ==========================================

// 1. «·„Ìœ· ÊÌ— «·Œ«’ »ÌﬂÌ (√Ê· Õ«Ã… ⁄‘«‰ Ì·ﬁÿ √Ì Error „‰ «··Ì »⁄œÂ)
app.UseMiddleware<RequestLoggerMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AsyncPlate API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

// 3. «·√„«‰ (·«“„ »⁄œ «·‹ Routing Êﬁ»· «·‹ Endpoints)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();