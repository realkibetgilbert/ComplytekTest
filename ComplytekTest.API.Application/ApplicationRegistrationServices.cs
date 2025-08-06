using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Interfaces;
using ComplytekTest.API.Application.Features.Department.Services;
using ComplytekTest.API.Application.Mapping.Dep;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Application.Mapping.Dep.Services;
using ComplytekTest.API.Application.Validators.Department;
using FluentValidation;
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
            services.AddScoped<IValidator<DepartmentToCreateDto>, CreateDepartmentDtoValidator>();
            services.AddScoped<IValidator<DepartmentToUpdateDto>, DepartmentToUpdateDtoValidator>();

            return services;
        }
    }
}
