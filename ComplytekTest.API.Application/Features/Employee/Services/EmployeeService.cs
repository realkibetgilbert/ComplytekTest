using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.Features.Employee.Interfaces;
using ComplytekTest.API.Application.Mapping.Emp.Interfaces;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ComplytekTest.API.Application.Features.Employee.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeMapper _employeeMapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<EmployeeToCreateDto> _employeeToCreateValidator;
        private readonly IValidator<EmployeeToUpdateDto> _employeeToUpdateValidator;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeMapper employeeMapper, IEmployeeRepository employeeRepository, IValidator<EmployeeToCreateDto> employeeToCreateValidator, IValidator<EmployeeToUpdateDto> employeeToUpdateValidator, ILogger<EmployeeService> logger)
        {
            _employeeMapper = employeeMapper;
            _employeeRepository = employeeRepository;
            _employeeToCreateValidator = employeeToCreateValidator;
            _employeeToUpdateValidator = employeeToUpdateValidator;
            _logger = logger;
        }
        public async Task<ApiResponse<EmployeeToDisplayDto>> CreateAsync(EmployeeToCreateDto employeeToCreateDto)
        {
            try
            {
                var validationResult = await _employeeToCreateValidator.ValidateAsync(employeeToCreateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<EmployeeToDisplayDto>.Failure(
                        message: "Validation failed. Please check your input.",
                        errors: errorMessages,
                        errorCode: ApiErrorCode.ValidationError
                    );
                }

                var employeeDomain = _employeeMapper.ToDomain(employeeToCreateDto);
                var createdEmployee = await _employeeRepository.CreateAsync(employeeDomain);
                var employeeToDisplay = _employeeMapper.ToDisplay(createdEmployee);

                return ApiResponse<EmployeeToDisplayDto>.Success(employeeToDisplay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating employee: {@Employee}", employeeToCreateDto);

                return ApiResponse<EmployeeToDisplayDto>.Failure(
                    message: "An unexpected error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<EmployeeToDisplayDto>?> DeleteAsync(long id)
        {
            try
            {
                var deletedEmployeee = await _employeeRepository.DeleteAsync(id);

                if (deletedEmployeee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion", id);
                    return ApiResponse<EmployeeToDisplayDto>.Failure(
                        message: "Department not found",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _employeeMapper.ToDisplay(deletedEmployeee);
                return ApiResponse<EmployeeToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID {EmployeeId}", id);
                return ApiResponse<EmployeeToDisplayDto>.Failure(
                    message: "An unexpected error occurred while deleting the employee.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<List<EmployeeToDisplayDto>>> GetAllAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync();
                var displayDtos = _employeeMapper.ToDisplay(employees);

                if (displayDtos == null || !displayDtos.Any())
                {
                    return ApiResponse<List<EmployeeToDisplayDto>>.Failure(
                        message: "No Employees found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                return ApiResponse<List<EmployeeToDisplayDto>>.Success(displayDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees.");

                return ApiResponse<List<EmployeeToDisplayDto>>.Failure(
                    message: "An unexpected error occurred.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<EmployeeToDisplayDto>> GetByIdAsync(long id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);

                if (employee == null)
                {
                    return ApiResponse<EmployeeToDisplayDto>.Failure(
                        message: "Employee not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _employeeMapper.ToDisplay(employee);

                return ApiResponse<EmployeeToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting employee with ID {id}");

                return ApiResponse<EmployeeToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }


        public async Task<ApiResponse<EmployeeToDisplayDto>?> UpdateAsync(long id, EmployeeToUpdateDto employeeToUpdateDto)
        {
            try
            {
                var validationResult = await _employeeToUpdateValidator.ValidateAsync(employeeToUpdateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed while updating employee: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<EmployeeToDisplayDto>.Failure(
                        message: "Validation failed. Check the input.",
                        errorCode: ApiErrorCode.ValidationError,
                        errors: errorMessages
                    );
                }

                var employeeDomain = _employeeMapper.ToDomain(employeeToUpdateDto);
                employeeDomain.Id = id;

                var updatedEmployee = await _employeeRepository.UpdateAsync(employeeDomain);

                if (updatedEmployee == null)
                {
                    return ApiResponse<EmployeeToDisplayDto>.Failure(
                        message: $"Employee with ID {id} not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _employeeMapper.ToDisplay(updatedEmployee);

                return ApiResponse<EmployeeToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating employee with ID {id}");

                return ApiResponse<EmployeeToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }
    }
}
