using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Core.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(long id);
        Task<Project> AddAsync(Project project);
        Task<Project?> UpdateAsync(Project project);
        Task<Project?> DeleteAsync(long id);
        Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(long employeeId);
        Task<bool> AssignEmployeeToProjectAsync(long employeeId, long projectId, string role);
        Task<bool> RemoveEmployeeFromProjectAsync(long employeeId, long projectId);
    }
}
