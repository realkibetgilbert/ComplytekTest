using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.Features.Employee.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Employee.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

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


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteEmployeeAsync(long id)
        {
            var response = await _employeeService.DeleteAsync(id);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }
    }
}
