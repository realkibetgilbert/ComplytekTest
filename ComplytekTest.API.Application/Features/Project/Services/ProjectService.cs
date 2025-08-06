using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.Common.Interfaces;
using ComplytekTest.API.Application.DTOs.EmployeeProjects;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Application.Features.Project.Interfaces;
using ComplytekTest.API.Application.Mapping.Proj.Interfaces;
using ComplytekTest.API.Application.Services.External;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ComplytekTest.API.Application.Features.Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectMapper _projectMapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProjectToCreateDto> _projectToCreateValidator;
        private readonly IValidator<ProjectToUpdateDto> _projectToUpdateValidator;
        private readonly ILogger<ProjectService> _logger;
        private readonly IRandomStringGenerator _randomStringGenerator;

        public ProjectService(IProjectMapper projectMapper, IProjectRepository projectRepository, IUnitOfWork unitOfWork, IValidator<ProjectToCreateDto> projectToCreateValidator, IValidator<ProjectToUpdateDto> projectToUpdateValidator, ILogger<ProjectService> logger, IRandomStringGenerator randomStringGenerator)
        {
            _projectMapper = projectMapper;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _projectToCreateValidator = projectToCreateValidator;
            _projectToUpdateValidator = projectToUpdateValidator;
            _logger = logger;
            _randomStringGenerator = randomStringGenerator;
        }

        public async Task<ApiResponse<string>> AssignEmployeeToProjectAsync(long projectId, AssignEmployeeProjectDto assignEmployeeProjectDto)
        {
            try
            {
                var success = await _projectRepository.AssignEmployeeToProjectAsync(assignEmployeeProjectDto.EmployeeId, projectId, assignEmployeeProjectDto.Role);

                if (!success)
                {
                    return ApiResponse<string>.Failure(
                        message: "Project is already assigned to the employee.",
                        errorCode: ApiErrorCode.Conflict
                    );
                }

                return ApiResponse<string>.Success("Employee successfully assigned to project.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning employee {EmployeeId} to project {ProjectId}", assignEmployeeProjectDto.EmployeeId, projectId);

                return ApiResponse<string>.Failure(
                    message: "An error occurred while assigning employee to project.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<ProjectToDisplayDto>> CreateAsync(ProjectToCreateDto projectToCreateDto)
        {
            try
            {
                var validationResult = await _projectToCreateValidator.ValidateAsync(projectToCreateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<ProjectToDisplayDto>.Failure(
                        message: "Validation failed. Please check your input.",
                        errors: errorMessages,
                        errorCode: ApiErrorCode.ValidationError
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                var projectDomain = _projectMapper.ToDomain(projectToCreateDto);

                var createdProject = await _projectRepository.AddAsync(projectDomain);

                var randomCode = await _randomStringGenerator.GenerateAsync();
                createdProject.ProjectCode = $"{randomCode}-{createdProject.Id}";

                await _projectRepository.UpdateAsync(createdProject);

                await _unitOfWork.CommitTransactionAsync();

                var projectToDisplay = _projectMapper.ToDisplay(createdProject);

                return ApiResponse<ProjectToDisplayDto>.Success(projectToDisplay);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                _logger.LogError(ex, "Unexpected error while creating project: {@Project}", projectToCreateDto);
                return ApiResponse<ProjectToDisplayDto>.Failure(
                    message: "An unexpected error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<ProjectToDisplayDto>?> DeleteAsync(long id)
        {
            try
            {
                var deletedProject = await _projectRepository.DeleteAsync(id);

                if (deletedProject == null)
                {
                    _logger.LogWarning("Project with ID {id} not found for deletion", id);
                    return ApiResponse<ProjectToDisplayDto>.Failure(
                        message: "Department not found",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _projectMapper.ToDisplay(deletedProject);
                return ApiResponse<ProjectToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID {ProjectId}", id);
                return ApiResponse<ProjectToDisplayDto>.Failure(
                    message: "An unexpected error occurred while deleting the project.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<List<ProjectToDisplayDto>>> GetAllAsync()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                var displayDtos = _projectMapper.ToDisplay(projects);

                if (displayDtos == null || !displayDtos.Any())
                {
                    return ApiResponse<List<ProjectToDisplayDto>>.Failure(
                        message: "No Employees found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                return ApiResponse<List<ProjectToDisplayDto>>.Success(displayDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching projects.");

                return ApiResponse<List<ProjectToDisplayDto>>.Failure(
                    message: "An unexpected error occurred.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }
        public async Task<ApiResponse<ProjectToDisplayDto>> GetByIdAsync(long id)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(id);

                if (project == null)
                {
                    return ApiResponse<ProjectToDisplayDto>.Failure(
                        message: "Project not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _projectMapper.ToDisplay(project);

                return ApiResponse<ProjectToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting project with ID {id}");

                return ApiResponse<ProjectToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<List<ProjectToDisplayDto>>> GetProjectsByEmployeeIdAsync(long employeeId)
        {
            try
            {
                var projects = await _projectRepository.GetProjectsByEmployeeIdAsync(employeeId);

                if (!projects.Any())
                {
                    return ApiResponse<List<ProjectToDisplayDto>>.Failure(
                        message: "No projects found for the given employee.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _projectMapper.ToDisplay(projects);

                return ApiResponse<List<ProjectToDisplayDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving projects for employee {EmployeeId}", employeeId);

                return ApiResponse<List<ProjectToDisplayDto>>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<string>> RemoveEmployeeFromProjectAsync(long projectId, RemoveEmployeeProjectDto removeEmployeeProjectDto)
        {
            try
            {
                var removed = await _projectRepository.RemoveEmployeeFromProjectAsync(removeEmployeeProjectDto.EmployeeId, projectId);

                if (!removed)
                {
                    return ApiResponse<string>.Failure(
                        message: "Employee is not assigned to this project.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                return ApiResponse<string>.Success("Employee was successfully removed from the project.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing employee {EmployeeId} from project {ProjectId}", removeEmployeeProjectDto.EmployeeId, projectId);

                return ApiResponse<string>.Failure(
                    message: "An internal error occurred while removing the employee from the project.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }

        public async Task<ApiResponse<ProjectToDisplayDto>?> UpdateAsync(long id, ProjectToUpdateDto projectToUpdateDto)
        {
            try
            {
                var validationResult = await _projectToUpdateValidator.ValidateAsync(projectToUpdateDto);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Validation failed while updating project: {@Errors}", validationResult.Errors);

                    var errorMessages = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return ApiResponse<ProjectToDisplayDto>.Failure(
                        message: "Validation failed. Check the input.",
                        errorCode: ApiErrorCode.ValidationError,
                        errors: errorMessages
                    );
                }

                var projectDomain = _projectMapper.ToDomain(projectToUpdateDto);
                projectDomain.Id = id;

                var updatedProject = await _projectRepository.UpdateAsync(projectDomain);

                if (updatedProject == null)
                {
                    return ApiResponse<ProjectToDisplayDto>.Failure(
                        message: $"Project with ID {id} not found.",
                        errorCode: ApiErrorCode.NotFound
                    );
                }

                var result = _projectMapper.ToDisplay(updatedProject);

                return ApiResponse<ProjectToDisplayDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating project with ID {id}");

                return ApiResponse<ProjectToDisplayDto>.Failure(
                    message: "An error occurred while processing your request.",
                    errorCode: ApiErrorCode.ServerError
                );
            }
        }


    }
}
