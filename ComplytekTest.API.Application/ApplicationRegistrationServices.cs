using ComplytekTest.API.Application.Features.Department.Interfaces;
using ComplytekTest.API.Application.Features.Department.Services;
using ComplytekTest.API.Application.Mapping.Dep;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Application.Mapping.Dep.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ComplytekTest.API.Application
{
    public static class ApplicationRegistrationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddTransient<IDepartmentMapper, DepartmentMapper>();
            services.AddAutoMapper(typeof(DepartmentMappingProfile));
            return services;
        }
    }
}
