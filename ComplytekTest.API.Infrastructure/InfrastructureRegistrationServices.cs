using ComplytekTest.API.Application.Interfaces;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;
using ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations;
using ComplytekTest.API.Infrastructure.Services.External;
using ComplytekTest.API.Infrastructure.Services.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ComplytekTest.API.Infrastructure
{
    public static class InfrastructureRegistrationServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpClient<IRandomStringGenerator, RandomStringGeneratorService>();
            services.AddScoped<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext!.Request;
                var baseUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(baseUri);
            });
            return services;
        }
    }
}
