using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.Features.Employee.Interfaces;
using ComplytekTest.API.Application.Features.Employee.Services;
using ComplytekTest.API.Application.Mapping.Emp.Interfaces;
using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
namespace ComplytekTest.API.Test.ServiceTests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeMapper> _mapperMock = new();
        private readonly Mock<IEmployeeRepository> _repoMock = new();
        private readonly Mock<IValidator<EmployeeToCreateDto>> _createValidatorMock = new();
        private readonly Mock<IValidator<EmployeeToUpdateDto>> _updateValidatorMock = new();
        private readonly Mock<ILogger<EmployeeService>> _loggerMock = new();

        private EmployeeService CreateService() =>
            new(_mapperMock.Object, _repoMock.Object, _createValidatorMock.Object, _updateValidatorMock.Object, _loggerMock.Object);

        [Fact]
        public async Task CreateAsync_ValidInput_ReturnsSuccess()
        {
            var dto = new EmployeeToCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1 };
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, CreatedOn = DateTime.UtcNow };
            var displayDto = new EmployeeToDisplayDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, DepartmentName = "IT", CreatedOn = employee.CreatedOn };

            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(employee);
            _repoMock.Setup(r => r.CreateAsync(employee)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.ToDisplay(employee)).Returns(displayDto);

            var service = CreateService();
            var result = await service.CreateAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApiErrorCode.None, result.ErrorCode);
            Assert.Equal(displayDto, result.Data);
        }

        [Fact]
        public async Task CreateAsync_InvalidInput_ReturnsValidationError()
        {
            var dto = new EmployeeToCreateDto();
            var errors = new[] { new ValidationFailure("FirstName", "Required") };
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult(errors));

            var service = CreateService();
            var result = await service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ValidationError, result.ErrorCode);
            Assert.Contains("Required", result.Errors[0]);
        }

        [Fact]
        public async Task CreateAsync_Exception_ReturnsServerError()
        {
            var dto = new EmployeeToCreateDto();
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ThrowsAsync(new Exception("fail"));

            var service = CreateService();
            var result = await service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_EmployeeExists_ReturnsSuccess()
        {
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, CreatedOn = DateTime.UtcNow };
            var displayDto = new EmployeeToDisplayDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, DepartmentName = "IT", CreatedOn = employee.CreatedOn };

            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.ToDisplay(employee)).Returns(displayDto);

            var service = CreateService();
            var result = await service.DeleteAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApiErrorCode.None, result.ErrorCode);
            Assert.Equal(displayDto, result.Data);
        }

        [Fact]
        public async Task DeleteAsync_EmployeeNotFound_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync((Employee)null);

            var service = CreateService();
            var result = await service.DeleteAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            var service = CreateService();
            var result = await service.DeleteAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_EmployeesExist_ReturnsSuccess()
        {
            var employees = new List<Employee> { new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, CreatedOn = DateTime.UtcNow } };
            var displayDtos = new List<EmployeeToDisplayDto> { new EmployeeToDisplayDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, DepartmentName = "IT", CreatedOn = employees[0].CreatedOn } };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.ToDisplay(employees)).Returns(displayDtos);

            var service = CreateService();
            var result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(ApiErrorCode.None, result.ErrorCode);
            Assert.Equal(displayDtos, result.Data);
        }

        [Fact]
        public async Task GetAllAsync_NoEmployees_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());
            _mapperMock.Setup(m => m.ToDisplay(It.IsAny<IEnumerable<Employee>>())).Returns(new List<EmployeeToDisplayDto>());

            var service = CreateService();
            var result = await service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            var service = CreateService();
            var result = await service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_EmployeeExists_ReturnsSuccess()
        {
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, CreatedOn = DateTime.UtcNow };
            var displayDto = new EmployeeToDisplayDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@doe.com", Salary = 1000, DepartmentId = 1, DepartmentName = "IT", CreatedOn = employee.CreatedOn };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.ToDisplay(employee)).Returns(displayDto);

            var service = CreateService();
            var result = await service.GetByIdAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApiErrorCode.None, result.ErrorCode);
            Assert.Equal(displayDto, result.Data);
        }

        [Fact]
        public async Task GetByIdAsync_EmployeeNotFound_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            var service = CreateService();
            var result = await service.GetByIdAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ThrowsAsync(new Exception("fail"));

            var service = CreateService();
            var result = await service.GetByIdAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task UpdateAsync_ValidInput_EmployeeExists_ReturnsSuccess()
        {
            var dto = new EmployeeToUpdateDto { FirstName = "Jane", LastName = "Smith", Email = "jane@smith.com", Salary = 2000, DepartmentId = 2 };
            var employee = new Employee { Id = 1, FirstName = "Jane", LastName = "Smith", Email = "jane@smith.com", Salary = 2000, DepartmentId = 2, CreatedOn = DateTime.UtcNow };
            var updatedEmployee = new Employee { Id = 1, FirstName = "Jane", LastName = "Smith", Email = "jane@smith.com", Salary = 2000, DepartmentId = 2, CreatedOn = DateTime.UtcNow };
            var displayDto = new EmployeeToDisplayDto { Id = 1, FirstName = "Jane", LastName = "Smith", Email = "jane@smith.com", Salary = 2000, DepartmentId = 2, DepartmentName = "HR", CreatedOn = updatedEmployee.CreatedOn };

            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(employee);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Employee>(e => e.Id == 1))).ReturnsAsync(updatedEmployee);
            _mapperMock.Setup(m => m.ToDisplay(updatedEmployee)).Returns(displayDto);

            var service = CreateService();
            var result = await service.UpdateAsync(1, dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApiErrorCode.None, result.ErrorCode);
            Assert.Equal(displayDto, result.Data);
        }

        [Fact]
        public async Task UpdateAsync_ValidInput_EmployeeNotFound_ReturnsNotFound()
        {
            var dto = new EmployeeToUpdateDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@smith.com",
                Salary = 2000,
                DepartmentId = 2
            };
            var employee = new Employee
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@smith.com",
                Salary = 2000,
                DepartmentId = 2,
                CreatedOn = DateTime.UtcNow
            };
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(employee);
            _repoMock.Setup(r => r.UpdateAsync(It.Is<Employee>(e => e.Id == 1))).ReturnsAsync((Employee)null);

            var service = CreateService();
            var result = await service.UpdateAsync(1, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }


        [Fact]
        public async Task UpdateAsync_InvalidInput_ReturnsValidationError()
        {
            var dto = new EmployeeToUpdateDto();
            var errors = new[] { new ValidationFailure("Email", "Invalid") };
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult(errors));

            var service = CreateService();
            var result = await service.UpdateAsync(1, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ValidationError, result.ErrorCode);
            Assert.Contains("Invalid", result.Errors[0]);
        }

        [Fact]
        public async Task UpdateAsync_Exception_ReturnsServerError()
        {
            var dto = new EmployeeToUpdateDto();
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ThrowsAsync(new Exception("fail"));

            var service = CreateService();
            var result = await service.UpdateAsync(1, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }
    }
}