using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Employee;
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
    }
}
