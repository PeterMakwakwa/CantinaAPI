using CantinaAPI.Repositories.Interfaces;
using CantinaAPI.Repositories;
using CantinaAPI.Services.Interfaces;
using CantinaAPI.Services;
using CantinaAPI.Data;
using CantinaAPI.Middlewares;
using CantinaAPI.Mappings;
using CantinaAPI.Shared;
using CantinaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text;
using CantinaAPI.CustomActionFilters;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CantinaAPI.Swagger;

namespace CantinaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            ConfigureServices(builder);
            #endregion

            var app = builder.Build();

            #region Configure Middleware
            ConfigureMiddleware(app);
            #endregion

            app.Run();
        }

        #region Service Configuration
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // Controllers
            services.AddControllers();
            // Database Context
            services.AddDbContext<CantinaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            // Identity
            services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<CantinaDbContext>()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();

            // Caching
            services.AddMemoryCache();
            services.Configure<CacheSettings>(builder.Configuration.GetSection("Cachetime"));
            services.AddSingleton<ICachingProvider, InMemoryCacheProvider>();

            // Repositories
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            // Services
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRateLimitingService, RateLimitingService>();
            services.AddScoped<RateLimitFilter>();

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMappersProfiles));

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            // Configure API Versioning
            services.AddApiVersioning(options =>
            {                
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
              .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Authentication
            ConfigureAuthentication(services, builder);
        }

        private static void ConfigureAuthentication(IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication(options =>
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.")))
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId is not configured.");
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
                googleOptions.CallbackPath = "/signin-google";
            });
        }
        #endregion

        #region Middleware Configuration
        private static void ConfigureMiddleware(WebApplication app)
        {
            // Development environment specific configuration
            if (app.Environment.IsDevelopment())
            {
                //
                var versiondescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                //
                app.UseSwagger();
                app.UseSwaggerUI(options=>
                {
                    foreach (var description in versiondescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            // Global exception handler
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Standard middleware
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // Route mapping
            app.MapControllers();
        }
        #endregion
    }
}