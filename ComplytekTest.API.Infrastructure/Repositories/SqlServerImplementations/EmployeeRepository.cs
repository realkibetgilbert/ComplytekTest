using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ComplytekTestDbContext _complytekTestDbContext;

        public EmployeeRepository(ComplytekTestDbContext complytekTestDbContext)
        {
            _complytekTestDbContext = complytekTestDbContext;
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
            await _complytekTestDbContext.Employees.AddAsync(employee);
            await _complytekTestDbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteAsync(long id)
        {
            var employee = await _complytekTestDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return null;

            _complytekTestDbContext.Employees.Remove(employee);
            await _complytekTestDbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _complytekTestDbContext.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(long id)
        {
            return await _complytekTestDbContext.Employees
         .Include(e => e.Department)
         .Include(e => e.EmployeeProjects)
         .ThenInclude(ep => ep.Project)
         .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(long departmentId)
        {
            return await _complytekTestDbContext.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .Where(e => e.DepartmentId == departmentId)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(long projectId)
        {
            return await _complytekTestDbContext.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .Where(e => e.EmployeeProjects.Any(ep => ep.ProjectId == projectId))
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            var employeeToUpdate = await _complytekTestDbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (employeeToUpdate != null)
            {
                employeeToUpdate.FirstName = employee.FirstName;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.Email = employee.Email;
                employeeToUpdate.Salary = employee.Salary;
                employeeToUpdate.DepartmentId = employee.DepartmentId;

                await _complytekTestDbContext.SaveChangesAsync();
            }

            return employeeToUpdate!;
        }
    }
}
