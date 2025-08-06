using ComplytekTest.API.Application.DTOs.Employee;
using FluentValidation;

namespace ComplytekTest.API.Application.Validators.Employee
{
    public class CreateEmployeeDtoValidator : AbstractValidator<EmployeeToCreateDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Salary)
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("Salary must be a positive number");

            RuleFor(x => x.DepartmentId)
                .NotNull()
                .GreaterThan(0).WithMessage("DepartmentId must be a positive number");
        }
    }

}
