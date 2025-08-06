using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Employee;

namespace ComplytekTest.API.Application.Features.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeToDisplayDto>> CreateAsync(EmployeeToCreateDto employeeToCreateDto);
        Task<ApiResponse<List<EmployeeToDisplayDto>>> GetAllAsync();
        Task<ApiResponse<EmployeeToDisplayDto>> GetByIdAsync(long id);
        Task<ApiResponse<EmployeeToDisplayDto>?> UpdateAsync(long id, EmployeeToUpdateDto employeeToUpdateDto);
        Task<ApiResponse<EmployeeToDisplayDto>?> DeleteAsync(long id);
    }
}
