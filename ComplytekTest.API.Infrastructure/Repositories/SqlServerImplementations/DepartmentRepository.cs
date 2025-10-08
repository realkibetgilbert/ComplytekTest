using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using ComplytekTest.API.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ComplytekTest.API.Infrastructure.Repositories.SqlServerImplementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ComplytekTestDbContext _compltekTestDbContext;

        public DepartmentRepository(ComplytekTestDbContext compltekTestDbContext)
        {
            _compltekTestDbContext = compltekTestDbContext;
        }
        public async Task<Department> CreateAsync(Department department)
        {

            await _compltekTestDbContext.Departments.AddAsync(department);
            await _compltekTestDbContext.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> DeleteAsync(long id)
        {
            var department = await _compltekTestDbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return null;

            _compltekTestDbContext.Departments.Remove(department);
            await _compltekTestDbContext.SaveChangesAsync();
            return department;
        }

        public async Task<IEnumerable<Department>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _compltekTestDbContext.Departments
              .Include(d => d.Employees)
              .Include(d => d.Projects)
              .AsNoTracking()
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(long id)
        {
            return await _compltekTestDbContext.Departments
              .Include(d => d.Employees)
              .Include(d => d.Projects)
              .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<decimal> GetTotalProjectBudgetAsync(long departmentId)
        {
            return await _compltekTestDbContext.Projects
                .Where(p => p.DepartmentId == departmentId)
                .SumAsync(p => p.Budget);
        }

        public async Task<Department?> UpdateAsync(Department department)
        {
            var departmentToUpdate = await _compltekTestDbContext.Departments.FirstOrDefaultAsync(d => d.Id == department.Id);

            if (departmentToUpdate != null)
            {
                departmentToUpdate.Name = department.Name;
                departmentToUpdate.OfficeLocation = department.OfficeLocation;
                departmentToUpdate.UpdatedOn = DateTime.UtcNow;

                await _compltekTestDbContext.SaveChangesAsync();
            }

            return departmentToUpdate!;
        }

        public async Task<int> CountAsync()
        {
            return await _compltekTestDbContext.Departments.CountAsync();
        }

    }
}
