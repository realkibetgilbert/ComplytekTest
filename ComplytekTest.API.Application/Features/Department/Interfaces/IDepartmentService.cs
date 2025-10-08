using ComplytekTest.API.Application.Common.Pagination;
using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.DTOs.Department;

namespace ComplytekTest.API.Application.Features.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<DepartmentToDisplayDto>> CreateAsync(DepartmentToCreateDto departmentToCreateDto);
        Task<PagedResponse<List<DepartmentToDisplayDto>>> GetAllAsync(PaginationFilter paginationFilter, string route);
        Task<ApiResponse<DepartmentToDisplayDto>> GetByIdAsync(long Id);
        Task<ApiResponse<DepartmentToDisplayDto>?> UpdateAsync(long id, DepartmentToUpdateDto departmentToUpdateDto);
        Task<ApiResponse<DepartmentToDisplayDto>?> DeleteAsync(long id);
        Task<ApiResponse<decimal>> GetTotalProjectBudgetAsync(long departmentId);
    }
}
