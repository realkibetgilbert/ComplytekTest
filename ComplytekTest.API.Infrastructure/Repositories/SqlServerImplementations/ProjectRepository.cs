using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;

namespace ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ComplytekTestDbContext _complytekTestDbContext;

        public ProjectRepository(ComplytekTestDbContext complytekTestDbContext)
        {
            _complytekTestDbContext = complytekTestDbContext;
        }
        public async Task<Project> AddAsync(Project project)
        {
            await _complytekTestDbContext.Projects.AddAsync(project);
            await _complytekTestDbContext.SaveChangesAsync();
            return project;
        }

        public Task<Project> AddProjectWithCodeAsync(Project project, Func<Task<string>> generateCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AssignEmployeeToProjectAsync(long employeeId, long projectId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetProjectsByEmployeeAsync(long employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEmployeeFromProjectAsync(long employeeId, long projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> UpdateAsync(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
