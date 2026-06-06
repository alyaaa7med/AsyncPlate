using AsyncPlate.API.Middlewares;
using AsyncPlate.Core.DTOs.Admin;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Offer;
using AsyncPlate.Core.DTOs.Order;
using AsyncPlate.Core.DTOs.Product;
using AsyncPlate.Core.DTOs.Recipe;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Repositories;
using AsyncPlate.Core.Interfaces.Services;
using AsyncPlate.Core.Mapping;
using AsyncPlate.Core.Services.Implementation;
using AsyncPlate.Core.Services.Interfaces;
using AsyncPlate.Core.Validators.Admin;
using AsyncPlate.Core.Validators.Authentication;
using AsyncPlate.Core.Validators.Category;
using AsyncPlate.Core.Validators.Inventory;
using AsyncPlate.Core.Validators.Offer;
using AsyncPlate.Core.Validators.Order;
using AsyncPlate.Core.Validators.Product;
using AsyncPlate.Core.Validators.Recipe;
using AsyncPlate.Core.Validators.Supplier;
using AsyncPlate.Infrastructure;
using AsyncPlate.Infrastructure.Data;
using AsyncPlate.Infrastructure.Data.Repositories;
using AsyncPlate.Infrastructure.Hubs;
using AsyncPlate.Infrastructure.Services;
using AsyncPlate.Infrastructure.Services.Settings;
using FluentValidation;
using Hangfire;
using Jose;
using Mailtrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database and Repositories from [infra + core ]

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
builder.Services.AddScoped<INotificationSender, SignalRNotificationSender>();
builder.Services.AddSignalR();

// Validations using FluentValidation [core ]

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



// Thrid Party and AutoMapper [infra + core + api ]

builder.Services.AddTransient<IEmailJobService, MailTrapEmailJobService>();

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


//hang fire
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
        ValidAudience = builder.Configuration["Jwt:Audience"]!,
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

// Building and Seedings
var app = builder.Build();

//seeding

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

// Middlewares [API ]

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
app.UseStaticFiles();

app.UseRouting();

// 3. «·√„«‰ (·«“„ »⁄œ «·‹ Routing Êﬁ»· «·‹ Endpoints)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");


app.Run();