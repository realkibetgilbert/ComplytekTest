using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Interfaces;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ComplytekTest.API.Application.Features.Department.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentMapper _departmentMapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentMapper departmentMapper, IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger)
        {
            _departmentMapper = departmentMapper;
            _departmentRepository = departmentRepository;
            _logger = logger;
        }
        public async Task<ApiResponse<DepartmentToDisplayDto>> CreateDepartmentAsync(DepartmentToCreateDto departmentToCreateDto)
        {
            try
            {

                var departmentDomain = _departmentMapper.ToDomain(departmentToCreateDto);

                var createdDepartment = await _departmentRepository.CreateAsync(departmentDomain);

                var departmentToDisplay = _departmentMapper.ToDisplay(createdDepartment);

                return ApiResponse<DepartmentToDisplayDto>.Success(departmentToDisplay);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department: {@Department}", departmentToCreateDto);
                return ApiResponse<DepartmentToDisplayDto>.Failure();
            }
        }
    }
}
