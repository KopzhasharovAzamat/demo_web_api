using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs;
using FluentValidation;

namespace demo_web_api.Validation;

public class ProjectEmployeeValidator : AbstractValidator<ProjectEmployeeDto> {
    public ProjectEmployeeValidator(IUnitOfWork unitOfWork) {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.")
            .MustAsync(async (id, cancellation) => await unitOfWork.Projects.ProjectExistsAsync(id))
            .WithMessage("Project with given ID does not exist.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.")
            .MustAsync(async (id, cancellation) => await unitOfWork.Employees.EmployeeExistsAsync(id))
            .WithMessage("Employee with given ID does not exist.");

        RuleFor(x => x)
            .MustAsync(
                async (model, cancellation) =>
                    !await unitOfWork.ProjectEmployees.ExistsProjectEmployeeAsync(model.ProjectId, model.EmployeeId)
            )
            .WithMessage("This employee is already assigned to this project.");
    }
}