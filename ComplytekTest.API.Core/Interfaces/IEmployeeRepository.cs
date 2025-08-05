using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(long id);
        Task<Employee> AddAsync(Employee employee);
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(long departmentId);
        Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(long projectId);
    }
}
