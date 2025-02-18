using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Web.Infrastructure.DBContext;
using Web.Infrastructure.DependencyInjection;
using Web.Infrastructure.Messaging;
using Web.Infrastructure.Middlewares;
using Web.Infrastructure.Repositories;

namespace Web.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        #region Add Repositories
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IClubMembershipRepository, ClubMembershipRepository>();
            return services;
        }
        #endregion
  
        
        #region Add Cache
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
            });
            return services;
        }
        #endregion

        #region Config DbContext
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
            return services;
        }
        #endregion

        #region HttpContext Accessor
        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
        #endregion

        #region Messaging
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddScoped<IMessageBus, RabbitMqMessageBus>();
            return services;
        }
        #endregion


        #region CircuitBreaker
        public static IServiceCollection AddPolly(this IServiceCollection services)
        {
            services.AddSingleton(PollyPolicyRegistry.GetPolicies());
            return services;
        }
        #endregion

        #region Authentication
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve JWT settings from the configuration
            var jwtSettings = configuration.GetSection("JwtSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"], // Get issuer from config
                    ValidAudience = jwtSettings["Audience"], // Get audience from config
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])) // Get secret key from config
                };
            });

            services.AddAuthorization();
            return services;
        }

        #endregion

        #region Config Swagger
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            // Add JWT Authentication to Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWT Auth API",
                    Version = "v1"
                });

                // Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' [space] and your token in the text input below.\n\nExample: `Bearer eyJhbGciOi...`",
                    In = ParameterLocation.Header
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
        #endregion
    }
}
