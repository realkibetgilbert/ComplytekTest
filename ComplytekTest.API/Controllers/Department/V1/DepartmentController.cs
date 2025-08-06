using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplytekTest.API.Controllers.Department.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

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


        [HttpGet]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var response = await _departmentService.GetAllAsync();

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

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


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteDepartmentAsync(long id)
        {
            var response = await _departmentService.DeleteAsync(id);

            return response.ErrorCode switch
            {
                ApiErrorCode.None => Ok(response),
                ApiErrorCode.NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }
        [HttpGet("{departmentId:long}/total-budget")]
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
