using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.DTOs.EmployeeProjects;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Application.Features.Project.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Project.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectToCreateDto">Project data to create.</param>
        /// <returns>API response with created project info.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectToCreateDto projectToCreateDto)
        {
            var response = await _projectService.CreateAsync(projectToCreateDto);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.ValidationError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>API response with list of projects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var response = await _projectService.GetAllAsync();

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets a project by its ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>API response with project info.</returns>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProjectByIdAsync(long id)
        {
            var response = await _projectService.GetByIdAsync(id);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Updates a project by its ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <param name="dto">Project data to update.</param>
        /// <returns>API response with updated project info.</returns>
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProjectAsync(long id, [FromBody] ProjectToUpdateDto dto)
        {
            var response = await _projectService.UpdateAsync(id, dto);

            return response?.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.ValidationError => BadRequest(response),
                ApiErrorCode.NotFound => NotFound(response),
                ApiErrorCode.ServerError => StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>API response indicating result.</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProjectAsync(long id)
        {
            var response = await _projectService.DeleteAsync(id);

            return response?.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Assigns an employee to a project.
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="assignEmployeeProjectDto">Employee assignment data.</param>
        /// <returns>API response indicating result.</returns>
        [HttpPost("{projectId:long}/assign-employee")]
        public async Task<IActionResult> AssignEmployeeToProjectAsync(long projectId, [FromBody] AssignEmployeeProjectDto assignEmployeeProjectDto)
        {
            var response = await _projectService.AssignEmployeeToProjectAsync(projectId, assignEmployeeProjectDto);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.Conflict => BadRequest(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="removeEmployeeProjectDto">Employee removal data.</param>
        /// <returns>API response indicating result.</returns>
        [HttpPost("{projectId:long}/remove-employee")]
        public async Task<IActionResult> RemoveEmployeeFromProjectAsync(long projectId, [FromBody] RemoveEmployeeProjectDto removeEmployeeProjectDto)
        {
            var response = await _projectService.RemoveEmployeeFromProjectAsync(projectId, removeEmployeeProjectDto);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets projects assigned to a specific employee.
        /// </summary>
        /// <param name="employeeId">Employee ID.</param>
        /// <returns>API response with list of projects.</returns>
        [HttpGet("by-employee/{employeeId:long}")]
        public async Task<IActionResult> GetProjectsByEmployeeIdAsync(long employeeId)
        {
            var response = await _projectService.GetProjectsByEmployeeIdAsync(employeeId);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

    }
}
