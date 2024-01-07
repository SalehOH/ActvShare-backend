using ActvShare.OptionsSetup;
using ActvShare.WebApi.Common;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;

namespace ActvShare.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                builder.Host.UseSerilog((context, configuration) =>
                   configuration.ReadFrom.Configuration(context.Configuration));
            }
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<ProblemDetailsFactory, ActvShareProblemDetailsFactorys>();
            services.AddCors();
            
            builder.Services.ConfigureOptions<SwaggerGenOptionsSetup>();
            builder.Services.ConfigureOptions<CoresOptionsSetup>();
            return services;
        }
    }
}
