using AutoMapper;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Dep
{
    public class DepartmentMappingProfile:Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<DepartmentToCreateDto, Department>().ReverseMap();
            CreateMap<Department, DepartmentToDisplayDto>().ReverseMap();
        }
    }
}
