using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Features.Department.Services;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace ComplytekTest.API.Test.ServiceTests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentMapper> _mapperMock;
        private readonly Mock<IDepartmentRepository> _repoMock;
        private readonly Mock<IValidator<DepartmentToCreateDto>> _createValidatorMock;
        private readonly Mock<IValidator<DepartmentToUpdateDto>> _updateValidatorMock;
        private readonly Mock<ILogger<DepartmentService>> _loggerMock;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _mapperMock = new Mock<IDepartmentMapper>();
            _repoMock = new Mock<IDepartmentRepository>();
            _createValidatorMock = new Mock<IValidator<DepartmentToCreateDto>>();
            _updateValidatorMock = new Mock<IValidator<DepartmentToUpdateDto>>();
            _loggerMock = new Mock<ILogger<DepartmentService>>();

            _service = new DepartmentService(
                _mapperMock.Object,
                _repoMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_Exception_ReturnsServerError()
        {
            var dto = new DepartmentToCreateDto { Name = "HR", OfficeLocation = "HQ" };
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ThrowsAsync(new Exception("fail"));

            var result = await _service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsSuccess()
        {
            var domain = new Department { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Employees = [], Projects = [] };
            var display = new DepartmentToDisplayDto { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = domain.CreatedOn };

            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(domain);
            _mapperMock.Setup(m => m.ToDisplay(domain)).Returns(display);

            var result = await _service.DeleteAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync((Department?)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            var result = await _service.DeleteAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_DepartmentsExist_ReturnsSuccess()
        {
            var departments = new List<Department>
            {
                new() { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Employees = [], Projects = [] }
            };
            var displayDtos = new List<DepartmentToDisplayDto>
            {
                new() { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = departments[0].CreatedOn }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);
            _mapperMock.Setup(m => m.ToDisplay(departments)).Returns(displayDtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(displayDtos, result.Data);
        }

        [Fact]
        public async Task GetAllAsync_NoDepartments_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Department>());
            _mapperMock.Setup(m => m.ToDisplay(It.IsAny<IEnumerable<Department>>())).Returns(new List<DepartmentToDisplayDto>());

            var result = await _service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            var result = await _service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsSuccess()
        {
            var domain = new Department { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Employees = [], Projects = [] };
            var display = new DepartmentToDisplayDto { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = domain.CreatedOn };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(domain);
            _mapperMock.Setup(m => m.ToDisplay(domain)).Returns(display);

            var result = await _service.GetByIdAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);

            var result = await _service.GetByIdAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ThrowsAsync(new Exception("fail"));

            var result = await _service.GetByIdAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetTotalProjectBudgetAsync_ExistingDepartment_ReturnsSuccess()
        {
            var domain = new Department { Id = 1, Name = "HR", OfficeLocation = "HQ", CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Employees = [], Projects = [] };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(domain);
            _repoMock.Setup(r => r.GetTotalProjectBudgetAsync(1)).ReturnsAsync(1000m);

            var result = await _service.GetTotalProjectBudgetAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(1000m, result.Data);
        }

        [Fact]
        public async Task GetTotalProjectBudgetAsync_NotFound_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department?)null);

            var result = await _service.GetTotalProjectBudgetAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetTotalProjectBudgetAsync_Exception_ReturnsServerError()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ThrowsAsync(new Exception("fail"));

            var result = await _service.GetTotalProjectBudgetAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task UpdateAsync_Exception_ReturnsServerError()
        {
            var dto = new DepartmentToUpdateDto { Name = "HR", OfficeLocation = "HQ" };
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ThrowsAsync(new Exception("fail"));

            var result = await _service.UpdateAsync(1, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }
    }
}
