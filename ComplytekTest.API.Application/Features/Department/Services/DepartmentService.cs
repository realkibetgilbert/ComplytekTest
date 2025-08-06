using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Interfaces;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ComplytekTest.API.Application.Features.Department.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentMapper _departmentMapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IValidator<DepartmentToCreateDto> _createDepartmentValidator;
        private readonly IValidator<DepartmentToUpdateDto> _updateDepartmentValidator;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentMapper departmentMapper, IDepartmentRepository departmentRepository, IValidator<DepartmentToCreateDto> createDepartmentValidator, IValidator<DepartmentToUpdateDto> updateDepartmentValidator, ILogger<DepartmentService> logger)
        {
            _departmentMapper = departmentMapper;
            _departmentRepository = departmentRepository;
            _createDepartmentValidator = createDepartmentValidator;
            _updateDepartmentValidator = updateDepartmentValidator;
            _logger = logger;
        }
        public async Task<ApiResponse<DepartmentToDisplayDto>> CreateAsync(DepartmentToCreateDto departmentToCreateDto)
        {
            try
            {
                var validationResult = await _createDepartmentValidator.ValidateAsync(departmentToCreateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<DepartmentToDisplayDto>.Failure(
                        message: "Validation failed. Please check your input.",
                        errors: errorMessages,
                        errorCode: ApiErrorCode.ValidationError
                    );
                }

                var departmentDomain = _departmentMapper.ToDomain(departmentToCreateDto);
                var createdDepartment = await _departmentRepository.CreateAsync(departmentDomain);
                var departmentToDisplay = _departmentMapper.ToDisplay(createdDepartment);

                return ApiResponse<DepartmentToDisplayDto>.Success(departmentToDisplay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating department: {@Department}", departmentToCreateDto);

                return ApiResponse<DepartmentToDisplayDto>.Failure(
                    message: "An unexpected error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<DepartmentToDisplayDto>?> DeleteAsync(long id)
        {
            try
            {
                var deletedDepartment = await _departmentRepository.DeleteAsync(id);

                if (deletedDepartment == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found for deletion", id);
                    return ApiResponse<DepartmentToDisplayDto>.Failure(
                        message: "Department not found",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _departmentMapper.ToDisplay(deletedDepartment);
                return ApiResponse<DepartmentToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting department with ID {DepartmentId}", id);
                return ApiResponse<DepartmentToDisplayDto>.Failure(
                    message: "An unexpected error occurred while deleting the department.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }


        public async Task<ApiResponse<List<DepartmentToDisplayDto>>> GetAllAsync()
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                var displayDtos = _departmentMapper.ToDisplay(departments);

                if (displayDtos == null || !displayDtos.Any())
                {
                    return ApiResponse<List<DepartmentToDisplayDto>>.Failure(
                        message: "No departments found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                return ApiResponse<List<DepartmentToDisplayDto>>.Success(displayDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");

                return ApiResponse<List<DepartmentToDisplayDto>>.Failure(
                    message: "An unexpected error occurred.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }


        public async Task<ApiResponse<DepartmentToDisplayDto>> GetByIdAsync(long id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);

                if (department == null)
                {
                    return ApiResponse<DepartmentToDisplayDto>.Failure(
                        message: "Department not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _departmentMapper.ToDisplay(department);

                return ApiResponse<DepartmentToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting department with ID {id}");

                return ApiResponse<DepartmentToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<decimal>> GetTotalProjectBudgetAsync(long departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(departmentId);

                if (department == null)
                {
                    return ApiResponse<decimal>.Failure(
                        message: "Department not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var totalBudget = await _departmentRepository.GetTotalProjectBudgetAsync(departmentId);

                return ApiResponse<decimal>.Success(totalBudget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting total budget for department ID {departmentId}");

                return ApiResponse<decimal>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }


        public async Task<ApiResponse<DepartmentToDisplayDto>?> UpdateAsync(long id, DepartmentToUpdateDto departmentToUpdateDto)
        {
            try
            {
                var validationResult = await _updateDepartmentValidator.ValidateAsync(departmentToUpdateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed while updating department: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<DepartmentToDisplayDto>.Failure(
                        message: "Validation failed. Check the input.",
                        errorCode: ApiErrorCode.ValidationError,
                        errors: errorMessages
                    );
                }

                var departmentDomain = _departmentMapper.ToDomain(departmentToUpdateDto);
                departmentDomain.Id = id;

                var updatedDepartment = await _departmentRepository.UpdateAsync(departmentDomain);

                if (updatedDepartment == null)
                {
                    return ApiResponse<DepartmentToDisplayDto>.Failure(
                        message: $"Department with ID {id} not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _departmentMapper.ToDisplay(updatedDepartment);

                return ApiResponse<DepartmentToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating department with ID {id}");

                return ApiResponse<DepartmentToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

    }
}
