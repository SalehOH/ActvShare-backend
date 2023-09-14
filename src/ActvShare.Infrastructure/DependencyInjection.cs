using ActvShare.Application.Common.Interfaces.Authentication;
using System.Text;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Infrastructure.Authentication;
using ActvShare.Infrastructure.Persistence;
using ActvShare.Infrastructure.Persistence.Repositories;
using ActvShare.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ActvShare.Application.Common.Interfaces.Services;

namespace ActvShare.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configration)
        {
            services
                .AddPersistance()
                .AddAuth(configration);

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
        public static IServiceCollection AddPersistance(this IServiceCollection services)
        {
            var DBServer = Environment.GetEnvironmentVariable("DBServer");
            var DBName = Environment.GetEnvironmentVariable("DBName");
            var DBPassword = Environment.GetEnvironmentVariable("DBPassword");

            services.AddDbContext<ApplicationContext>(op => 
                op.UseSqlServer($"Data Source={DBServer};Initial Catalog={DBName};User Id=sa;Password={DBPassword};Encrypt=false"));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
         
            return services;
        }
        
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configration)
        {
            var jwtSettings = new JwtSettings();
            configration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });

            return services;
        }
    }
}