using ComplytekTest.API.Application.Common;
using ComplytekTest.API.Application.Common.Interfaces;
using ComplytekTest.API.Application.DTOs.Project;
using ComplytekTest.API.Application.Features.Project.Services;
using ComplytekTest.API.Application.Mapping.Proj.Interfaces;
using ComplytekTest.API.Application.Services.External;
using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Core.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ComplytekTest.API.Test.ServiceTests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectMapper> _mapperMock;
        private readonly Mock<IProjectRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<ProjectToCreateDto>> _createValidatorMock;
        private readonly Mock<IValidator<ProjectToUpdateDto>> _updateValidatorMock;
        private readonly Mock<ILogger<ProjectService>> _loggerMock;
        private readonly Mock<IRandomStringGenerator> _randomStringGeneratorMock;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _mapperMock = new Mock<IProjectMapper>();
            _repoMock = new Mock<IProjectRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _createValidatorMock = new Mock<IValidator<ProjectToCreateDto>>();
            _updateValidatorMock = new Mock<IValidator<ProjectToUpdateDto>>();
            _loggerMock = new Mock<ILogger<ProjectService>>();
            _randomStringGeneratorMock = new Mock<IRandomStringGenerator>();

            _service = new ProjectService(
                _mapperMock.Object,
                _repoMock.Object,
                _uowMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _loggerMock.Object,
                _randomStringGeneratorMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ReturnsSuccess_WhenValid()
        {
            var dto = new ProjectToCreateDto { Name = "Test", Budget = 100, DepartmentId = 1 };
            var validationResult = new ValidationResult();
            var domain = new Project { Id = 1, Name = "Test", Budget = 100, DepartmentId = 1 };
            var created = new Project { Id = 1, Name = "Test", Budget = 100, DepartmentId = 1 };
            var display = new ProjectToDisplayDto { Id = 1, Name = "Test", Budget = 100, DepartmentId = 1 };

            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(domain);
            _repoMock.Setup(r => r.AddAsync(domain)).ReturnsAsync(created);
            _randomStringGeneratorMock.Setup(r => r.GenerateAsync()).ReturnsAsync("RANDOM");
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Project>())).ReturnsAsync(created);
            _uowMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.ToDisplay(created)).Returns(display);

            var result = await _service.CreateAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFailure_WhenValidationFails()
        {
            var dto = new ProjectToCreateDto { Name = "", Budget = 0, DepartmentId = 0 };
            var errors = new[] { new ValidationFailure("Name", "Required") };
            var validationResult = new ValidationResult(errors);

            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);

            var result = await _service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ValidationError, result.ErrorCode);
            Assert.Contains("Required", result.Errors[0]);
        }

        [Fact]
        public async Task CreateAsync_RollsBackAndReturnsFailure_OnException()
        {
            var dto = new ProjectToCreateDto { Name = "Test", Budget = 100, DepartmentId = 1 };
            var validationResult = new ValidationResult();

            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _uowMock.Setup(u => u.BeginTransactionAsync()).ThrowsAsync(new Exception("DB error"));
            _uowMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenProjectDeleted()
        {
            var id = 1L;
            var deleted = new Project { Id = id, Name = "Test", Budget = 100, DepartmentId = 1 };
            var display = new ProjectToDisplayDto { Id = id, Name = "Test", Budget = 100, DepartmentId = 1 };

            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(deleted);
            _mapperMock.Setup(m => m.ToDisplay(deleted)).Returns(display);

            var result = await _service.DeleteAsync(id);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFailure_WhenProjectNotFound()
        {
            var id = 1L;
            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync((Project)null);

            var result = await _service.DeleteAsync(id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFailure_OnException()
        {
            var id = 1L;
            _repoMock.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new Exception("Error"));

            var result = await _service.DeleteAsync(id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSuccess_WhenProjectsExist()
        {
            var projects = new List<Project> { new Project { Id = 1, Name = "Test", Budget = 100, DepartmentId = 1 } };
            var displayDtos = new List<ProjectToDisplayDto> { new ProjectToDisplayDto { Id = 1, Name = "Test", Budget = 100, DepartmentId = 1 } };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);
            _mapperMock.Setup(m => m.ToDisplay(projects)).Returns(displayDtos);

            var result = await _service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(displayDtos, result.Data);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFailure_WhenNoProjects()
        {
            var projects = new List<Project>();
            var displayDtos = new List<ProjectToDisplayDto>();

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);
            _mapperMock.Setup(m => m.ToDisplay(projects)).Returns(displayDtos);

            var result = await _service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFailure_OnException()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("Error"));

            var result = await _service.GetAllAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSuccess_WhenProjectFound()
        {
            var id = 1L;
            var project = new Project { Id = id, Name = "Test", Budget = 100, DepartmentId = 1 };
            var display = new ProjectToDisplayDto { Id = id, Name = "Test", Budget = 100, DepartmentId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);
            _mapperMock.Setup(m => m.ToDisplay(project)).Returns(display);

            var result = await _service.GetByIdAsync(id);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFailure_WhenProjectNotFound()
        {
            var id = 1L;
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Project)null);

            var result = await _service.GetByIdAsync(id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFailure_OnException()
        {
            var id = 1L;
            _repoMock.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new Exception("Error"));

            var result = await _service.GetByIdAsync(id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenValid()
        {
            var id = 1L;
            var dto = new ProjectToUpdateDto { Name = "Updated", Budget = 200, DepartmentId = 2 };
            var validationResult = new ValidationResult();
            var domain = new Project { Id = id, Name = "Updated", Budget = 200, DepartmentId = 2 };
            var updated = new Project { Id = id, Name = "Updated", Budget = 200, DepartmentId = 2 };
            var display = new ProjectToDisplayDto { Id = id, Name = "Updated", Budget = 200, DepartmentId = 2 };

            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(domain);
            _repoMock.Setup(r => r.UpdateAsync(domain)).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.ToDisplay(updated)).Returns(display);

            var result = await _service.UpdateAsync(id, dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(display, result.Data);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFailure_WhenValidationFails()
        {
            var id = 1L;
            var dto = new ProjectToUpdateDto { Name = "", Budget = 0, DepartmentId = 0 };
            var errors = new[] { new ValidationFailure("Name", "Required") };
            var validationResult = new ValidationResult(errors);

            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ValidationError, result.ErrorCode);
            Assert.Contains("Required", result.Errors[0]);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFailure_WhenProjectNotFound()
        {
            var id = 1L;
            var dto = new ProjectToUpdateDto { Name = "Updated", Budget = 200, DepartmentId = 2 };
            var validationResult = new ValidationResult();
            var domain = new Project { Id = id, Name = "Updated", Budget = 200, DepartmentId = 2 };

            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _mapperMock.Setup(m => m.ToDomain(dto)).Returns(domain);
            _repoMock.Setup(r => r.UpdateAsync(domain)).ReturnsAsync((Project)null);

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.NotFound, result.ErrorCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFailure_OnException()
        {
            var id = 1L;
            var dto = new ProjectToUpdateDto { Name = "Updated", Budget = 200, DepartmentId = 2 };
            var validationResult = new ValidationResult();

            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _mapperMock.Setup(m => m.ToDomain(dto)).Throws(new Exception("Error"));

            var result = await _service.UpdateAsync(id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApiErrorCode.ServerError, result.ErrorCode);
        }
    }
}
