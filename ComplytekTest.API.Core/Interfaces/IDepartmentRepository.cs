using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Core.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(long id);
        Task<Department> CreateAsync(Department department);
        Task<Department?> UpdateAsync(Department department);
        Task<Department?> DeleteAsync(long id);
        Task<decimal> GetTotalProjectBudgetAsync(long departmentId);
    }
}
