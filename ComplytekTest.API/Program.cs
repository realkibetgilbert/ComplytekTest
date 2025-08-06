using ComplytekTest.API.Application;
using ComplytekTest.API.Extensions;
using ComplytekTest.API.Infrastructure;
using ComplytekTest.API.Infrastructure.Persistance;
using ComplytekTest.API.Infrastructure.Seed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ComplytekTest API",
        Description = """
            This RESTful API allows management of employees, departments, and projects in a company.

            CRUD operations for:
            - Employees
            - Departments
            - Projects

            Project-specific features:
            - Assign or remove employees from projects with roles
            - View all projects assigned to an employee
            - View total budget of projects handled by a department

            Project creation includes unique project code generation via an external Random Code Generator API.
            This process ensures transactional consistency by using database transactions.

            Built with .NET 9 and SQL Server (EF Core code-first), deployable with Docker Compose.
        """
    });


    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<ComplytekTestDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ComplytekTestConnection")
    )
);

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Complytek Test API v1");
    c.DocumentTitle = "ComplytekTest API Documentation";
    c.RoutePrefix = string.Empty;
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ComplytekTestDbContext>();

    const int maxRetries = 15;
    var delay = TimeSpan.FromSeconds(5);

    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            logger.LogInformation(" Attempting to migrate and seed database. Attempt {Attempt}/{MaxRetries}", attempt, maxRetries);

            await context.Database.MigrateAsync();
            await DatabaseSeeder.SeedAsync(context);

            logger.LogInformation(" Database migration and seeding completed.");
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, " Database migration/seeding failed on attempt {Attempt}/{MaxRetries}", attempt, maxRetries);

            if (attempt == maxRetries)
            {
                logger.LogCritical(" Maximum retry attempts reached. Unable to migrate/seed database.");
                throw;
            }

            await Task.Delay(delay);
            delay *= 2;
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
