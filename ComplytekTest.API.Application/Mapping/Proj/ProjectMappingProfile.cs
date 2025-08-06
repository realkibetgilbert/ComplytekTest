using AutoMapper;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Proj
{
    public class ProjectMappingProfile:Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectToDisplayDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<ProjectToCreateDto, Project>();
            CreateMap<ProjectToUpdateDto, Project>();
        }
    }
}
