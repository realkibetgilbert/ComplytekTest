using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AssignEmployeeToProjectAsync(long employeeId, long projectId, string role)
        {
            var existingAssignment = await _complytekTestDbContext.EmployeeProjects.FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (existingAssignment != null) return false;

            var employeeProject = new EmployeeProject
            {
                EmployeeId = employeeId,
                ProjectId = projectId,
                Role = role
            };

            await _complytekTestDbContext.EmployeeProjects.AddAsync(employeeProject);
            await _complytekTestDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Project?> DeleteAsync(long id)
        {
            var existingProject = await _complytekTestDbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null) return null;

            _complytekTestDbContext.Projects.Remove(existingProject);
            await _complytekTestDbContext.SaveChangesAsync();

            return existingProject;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _complytekTestDbContext.Projects
             .Include(p => p.Department)
             .Include(p => p.EmployeeProjects)
             .ThenInclude(ep => ep.Employee)
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(long id)
        {
            return await _complytekTestDbContext.Projects
                .Include(p => p.Department)
                .Include(p => p.EmployeeProjects)
                .ThenInclude(ep => ep.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(long employeeId)
        {
            return await _complytekTestDbContext.Projects
            .Include(p => p.Department)
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .Where(p => p.EmployeeProjects.Any(ep => ep.EmployeeId == employeeId))
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<bool> RemoveEmployeeFromProjectAsync(long employeeId, long projectId)
        {
            var employeeProject = await _complytekTestDbContext.EmployeeProjects.FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (employeeProject == null) return false;

            _complytekTestDbContext.EmployeeProjects.Remove(employeeProject);
            await _complytekTestDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Project?> UpdateAsync(Project project)
        {
            var existingProject = await _complytekTestDbContext.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Budget = project.Budget;
                existingProject.ProjectCode = project.ProjectCode;
                existingProject.DepartmentId = project.DepartmentId;
                existingProject.UpdatedOn = DateTime.UtcNow;

                await _complytekTestDbContext.SaveChangesAsync();
            }

            return existingProject!;
        }
    }
}
