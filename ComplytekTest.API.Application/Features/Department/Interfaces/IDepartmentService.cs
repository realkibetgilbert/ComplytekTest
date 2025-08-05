using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;

namespace ComplytekTest.API.Application.Features.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<DepartmentToDisplayDto>> CreateDepartmentAsync(DepartmentToCreateDto departmentToCreateDto);
    }
}
