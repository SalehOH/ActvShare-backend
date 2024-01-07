using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace ActvShare.OptionsSetup;

public class CoresOptionsSetup : IConfigureNamedOptions<CorsOptions>
{
    public void Configure(CorsOptions options)
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://frontend:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    }

    public void Configure(string? name, CorsOptions options) => Configure(options);
}