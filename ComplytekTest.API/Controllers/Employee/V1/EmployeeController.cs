using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.Features.Employee.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Employee.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employeeToCreateDto">Employee data to create.</param>
        /// <returns>API response with created employee info.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeToCreateDto employeeToCreateDto)
        {
            var response = await _employeeService.CreateAsync(employeeToCreateDto);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.ValidationError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <returns>API response with list of employees.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            var response = await _employeeService.GetAllAsync();

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Gets an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>API response with employee info.</returns>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(long id)
        {
            var response = await _employeeService.GetByIdAsync(id);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// Updates an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <param name="dto">Employee data to update.</param>
        /// <returns>API response with updated employee info.</returns>
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateEmployeeAsync(long id, [FromBody] EmployeeToUpdateDto dto)
        {
            var response = await _employeeService.UpdateAsync(id, dto);

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
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>API response indicating result.</returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteEmployeeAsync(long id)
        {
            var response = await _employeeService.DeleteAsync(id);

            return response?.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }
    }
}
