using AsyncPlate.API.Middlewares;
using AsyncPlate.Application.DTOs.Admin;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Mapping;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.Validators.Admin;
using AsyncPlate.Application.Validators.Authentication;
using AsyncPlate.Application.Validators.Category;
using AsyncPlate.Application.Validators.Inventory;
using AsyncPlate.Application.Validators.Offer;
using AsyncPlate.Application.Validators.Order;
using AsyncPlate.Application.Validators.Product;
using AsyncPlate.Application.Validators.Recipe;
using AsyncPlate.Application.Validators.Supplier;
using AsyncPlate.Domain.Entities;
using AsyncPlate.Infrastructure.Data;
using AsyncPlate.Infrastructure.Data.Repositories;
using AsyncPlate.Infrastructure.Hubs;
using AsyncPlate.Infrastructure.Services;
using AsyncPlate.Infrastructure.Services.Jobs;
using AsyncPlate.Infrastructure.Services.Settings;
using FluentValidation;
using Hangfire;
using Jose;
using Mailtrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database and Repositories from [infra + application ]

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IKitchenChefRepo, KitchenChefRepo>();
builder.Services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
builder.Services.AddScoped<IOneTimeTokenRepo, OneTimeTokenRepo>();
builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IRecipeRepo, RecipeRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IOfferRepo, OfferRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductExtraRepo, ProductExtraRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
builder.Services.AddScoped<IOrderExtraItemRepo, OrderExtraItemRepo>();
builder.Services.AddScoped<INotificationRepo, NotificationRepo>();


builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<INotificationSender, SignalRNotificationSender>();
builder.Services.AddScoped<IOfferJob, OfferJob>();
builder.Services.AddScoped<IOrderJob, OrderJob>();

builder.Services.AddSignalR();

// Validations using FluentValidation [application ]

builder.Services.AddTransient<IValidator<SignupAppUserRequestDTO>, SignupAppUserRequestValidator>();   
builder.Services.AddTransient<IValidator<SignupCustomerRequestDTO>, SignupCustomerRequestValidator>();
builder.Services.AddTransient<IValidator<CreateAdminRequestDTO>, CreateAdminRequestValidator>();
builder.Services.AddTransient<IValidator<SignupKitchenChefRequestDTO>, SignupKitchenChefRequestValidator>();
builder.Services.AddTransient<IValidator<LoginRequestDTO>, LoginRequestValidator>();
builder.Services.AddTransient<IValidator<ForgetPasswordRequestDTO>, ForgetPasswordRequestValidator>();
builder.Services.AddTransient<IValidator<ResetPasswordRequestDTO>, ResetPasswordRequestValidator>();
builder.Services.AddTransient<IValidator<RefreshTokenRequestDTO>, RefreshTokenRequestValidator>();
builder.Services.AddTransient<IValidator<AddInventoryRequestDTO>, AddInventoryRequestValidator>();
builder.Services.AddTransient<IValidator<UpdateInventoryRequestDTO>, UpdateInventoryRequestValidator>();
builder.Services.AddTransient<IValidator<AddSupplierRequestDTO>, AddSupplierRequestValidator>();
builder.Services.AddTransient<IValidator<UpdateSupplierRequestDTO>, UpdateSupplierRequestValidator>();
builder.Services.AddTransient<IValidator<AddRecipeRequestDTO>, AddRecipeRequestValidator>();
builder.Services.AddTransient<IValidator<UpdateRecipeRequestDTO>, UpdateRecipeRequestValidator>();
builder.Services.AddTransient<IValidator<AddCategoryRequestDTO>, AddCategoryRequestValidator>();
builder.Services.AddTransient<IValidator<AddProductRequestDTO>, AddProductRequestValidator>();
builder.Services.AddTransient<IValidator<MakeOrderRequestDTO>, MakeOrderRequestValidator>();
builder.Services.AddTransient<IValidator<MakeOrderRequestDTO>, MakeOrderRequestValidator>();
builder.Services.AddTransient<IValidator<OrderItemRequestDTO>, OrderItemRequestValidator>();
builder.Services.AddTransient<IValidator<OrderExtraItemRequestDTO>, OrderExtraItemRequestValidator>();
builder.Services.AddTransient<IValidator<AddOfferRequestDTO>, AddOfferRequestValidator>();
builder.Services.AddTransient<IValidator<UpdateOfferRequestDTO>, UpdateOfferRequestValidator>();



// Thrid Party and AutoMapper [infra + application + api ]

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
    cfg.AddProfile<AuthProfile>();
    cfg.AddProfile<CustomerProfile>();
    cfg.AddProfile<KitchenChefProfile>();
    cfg.AddProfile<SupplierProfile>();
    cfg.AddProfile<InventoryProfile>();
    cfg.AddProfile<AdminProfile>();
    cfg.AddProfile<RecipeProfile>();
    cfg.AddProfile<ProductProfile>();
    cfg.AddProfile<CategoryProfile>();
    cfg.AddProfile<OfferProfile>();
    cfg.AddProfile<OrderProfile>();
}, typeof(Program));

//signalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

//hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(connectionString)
);

builder.Services.AddHangfireServer();


// API Swagger [infra ]

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

// Identity and JWT [ infra + core ]

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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
    // it is for signalR to allow passing token in query string for websocket
    // connections (because headers are not supported in websocket)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/hubs/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

var app = builder.Build();

//seeding instead of make seprate role service 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "SuperAdmin","Admin", "Guest", "KitchenChef", "Customer" };
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

using (var scope = app.Services.CreateScope())
{
    var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJob.AddOrUpdate<ReportJob>(
        "daily-report",
        job => job.ExecuteAsync(),
        Cron.Daily(21, 0));
}

using (var scope = app.Services.CreateScope())
{
    var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJob.AddOrUpdate<IInventoryJob>(
    "daily-low-stock-email",
    job => job.SendLowStockSuppliersEmail(),
    Cron.Daily(21, 0));

}

// Middlewares [API ]

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AsyncPlate API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// 3. «·√„«‰ (·«“„ »⁄œ «·‹ Routing Êﬁ»· «·‹ Endpoints)
app.UseCors("cors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");


app.Run();