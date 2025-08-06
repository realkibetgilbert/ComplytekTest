using AutoMapper;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Emp
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<EmployeeToCreateDto, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeToDisplayDto>().ReverseMap();
            CreateMap<EmployeeToUpdateDto, Employee>().ReverseMap();
        }
    }
}
