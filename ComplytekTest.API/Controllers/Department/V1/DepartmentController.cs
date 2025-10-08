using ComplytekTest.API.Application.Common.Pagination;
using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Department.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="departmentToCreateDto">Department data to create.</param>
        /// <returns>API response with created department info.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] DepartmentToCreateDto departmentToCreateDto)
        {
            var response = await _departmentService.CreateAsync(departmentToCreateDto);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.ValidationError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>API response with list of departments.</returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsAsync([FromQuery] PaginationFilter filter)
        {
            var route = Request?.Path.Value ?? string.Empty; 

            var response = await _departmentService.GetAllAsync(filter, route);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets a department by ID.
        /// </summary>
        /// <param name="id">Department ID.</param>
        /// <returns>API response with department info.</returns>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(long id)
        {
            var response = await _departmentService.GetByIdAsync(id);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Updates a department by ID.
        /// </summary>
        /// <param name="id">Department ID.</param>
        /// <param name="dto">Department data to update.</param>
        /// <returns>API response with updated department info.</returns>
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateDepartmentAsync(long id, [FromBody] DepartmentToUpdateDto dto)
        {
            var response = await _departmentService.UpdateAsync(id, dto);

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
        /// Deletes a department by ID.
        /// </summary>
        /// <param name="id">Department ID.</param>
        /// <returns>API response indicating result.</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteDepartmentAsync(long id)
        {
            var response = await _departmentService.DeleteAsync(id);

            return response?.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets the total project budget for a department.
        /// </summary>
        /// <param name="departmentId">Department ID.</param>
        /// <returns>API response with total project budget.</returns>
        [HttpGet("{departmentId:long}/total-project-budget")]
        public async Task<IActionResult> GetTotalProjectBudgetAsync(long departmentId)
        {
            var response = await _departmentService.GetTotalProjectBudgetAsync(departmentId);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

    }
}
