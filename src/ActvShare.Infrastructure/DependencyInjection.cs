using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Infrastructure.Authentication;
using ActvShare.Infrastructure.Persistence;
using ActvShare.Infrastructure.Persistence.Repositories;
using ActvShare.Infrastructure.Services;
using ActvShare.Application.Common.Interfaces.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ActvShare.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configration
            )
        {
            services
                .AddPersistence(configration)
                .AddAuth(configration);

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            var DBServer = Environment.GetEnvironmentVariable("DBServer");
            var DBName = Environment.GetEnvironmentVariable("DBName");
            var DBPassword = Environment.GetEnvironmentVariable("DBPassword");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer($"Data Source={DBServer};Initial Catalog={DBName};User Id=sa;Password={DBPassword};Encrypt=false"));
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();

            services.AddSignalR();
            return services;
        }


        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configration)
        {
            var jwtSettings = new JwtSettings();
            configration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = new TimeSpan(0, 0, 0, 5)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (
                                (path.StartsWithSegments("/hubs/chatHub"))
                                || path.StartsWithSegments("/hubs/notification")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

    }
}