using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Proj.Interfaces
{
    public interface IProjectMapper
    {
        Project ToDomain(ProjectToCreateDto projectToCreateDto);
        ProjectToDisplayDto ToDisplay(Project project);
        List<ProjectToDisplayDto> ToDisplay(IEnumerable<Project> projects);
        Project ToDomain(ProjectToUpdateDto projectToUpdateDto);
    }
}
