using ComplytekTest.API.Application.DTOs.Project;
using FluentValidation;

namespace ComplytekTest.API.Application.Validators.Project
{
    public class UpdateProjectDtoValidator : AbstractValidator<ProjectToUpdateDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Budget)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Budget must be a positive number");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("DepartmentId must be a positive number");
        }
    }
}
