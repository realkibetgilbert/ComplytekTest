using ComplytekTest.API.Application.DTOs.Department;
using FluentValidation;

namespace ComplytekTest.API.Application.Validators.Department
{
    public class CreateDepartmentDtoValidator : AbstractValidator<DepartmentToCreateDto>
    {
        public CreateDepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MinimumLength(3).WithMessage("Name must be at least 3 characters.")
             .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.OfficeLocation)
                .NotEmpty().WithMessage("Office location is required.")
                .MinimumLength(3).WithMessage("Office location must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Office location must not exceed 100 characters.");
        }
    }
}
