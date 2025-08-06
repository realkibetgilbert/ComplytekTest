using ComplytekTest.API.Application.Common.Interfaces;
using ComplytekTest.API.Application.Services.External;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;
using ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations;
using ComplytekTest.API.Infrastructure.Services.External;
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
            return services;
        }
    }
}
