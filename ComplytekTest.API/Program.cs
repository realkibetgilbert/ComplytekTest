using ComplytekTest.API.Application;
using ComplytekTest.API.Extensions;
using ComplytekTest.API.Infrastructure;
using ComplytekTest.API.Infrastructure.Persistance;
using ComplytekTest.API.Infrastructure.Seed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddDbContext<ComplytekTestDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ComplytekTestConnection")
    )
);
builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

    options.ApiVersionReader = new UrlSegmentApiVersionReader(); 
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ComplytekTestDbContext>();
        await context.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
