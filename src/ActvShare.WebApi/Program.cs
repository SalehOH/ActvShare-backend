using ActvShare.Application;
using ActvShare.Infrastructure;
using ActvShare.Infrastructure.Hubs;
using ActvShare.WebApi;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSerilogRequestLogging();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
    RequestPath = "/images"
});

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("hubs/chatHub");

app.MapControllers();
app.Run();