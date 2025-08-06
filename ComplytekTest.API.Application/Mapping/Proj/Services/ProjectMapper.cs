using AutoMapper;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Application.Mapping.Proj.Interfaces;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Proj.Services
{
    public class ProjectMapper : IProjectMapper
    {
        private readonly IMapper _mapper;

        public ProjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ProjectToDisplayDto ToDisplay(Project project)
        {
            return _mapper.Map<ProjectToDisplayDto>(project);
        }

        public List<ProjectToDisplayDto> ToDisplay(IEnumerable<Project> projects)
        {
            return _mapper.Map<List<ProjectToDisplayDto>>(projects);
        }

        public Project ToDomain(ProjectToCreateDto projectToCreateDto)
        {
            return _mapper.Map<Project>(projectToCreateDto);
        }

        public Project ToDomain(ProjectToUpdateDto projectToUpdateDto)
        {
            return _mapper.Map<Project>(projectToUpdateDto);
        }
    }
}
