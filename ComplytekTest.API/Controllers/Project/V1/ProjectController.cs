using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Application.Features.Project.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Project.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
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


    }
}
