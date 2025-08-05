using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations;
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

            return services;
        }
    }
}
