using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.DTOs.EmployeeProjects;
using ComplytekTest.API.Application.DTOs.Project;

namespace ComplytekTest.API.Application.Features.Project.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<ProjectToDisplayDto>> CreateAsync(ProjectToCreateDto projectToCreateDto);
        Task<ApiResponse<List<ProjectToDisplayDto>>> GetAllAsync();
        Task<ApiResponse<ProjectToDisplayDto>> GetByIdAsync(long id);
        Task<ApiResponse<ProjectToDisplayDto>?> UpdateAsync(long id, ProjectToUpdateDto E);
        Task<ApiResponse<ProjectToDisplayDto>?> DeleteAsync(long id);
        Task<ApiResponse<List<ProjectToDisplayDto>>> GetProjectsByEmployeeIdAsync(long employeeId);
        Task<ApiResponse<string>> AssignEmployeeToProjectAsync(long projectId, AssignEmployeeProjectDto assignEmployeeProjectDto);
        Task<ApiResponse<string>> RemoveEmployeeFromProjectAsync( long projectId,RemoveEmployeeProjectDto removeEmployeeProjectDto);
    }
}
