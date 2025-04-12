using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using FluentValidation;

namespace demo_web_api.BLL.Validation;

public class ProjectEmployeeValidator : AbstractValidator<ProjectEmployee> {
    public ProjectEmployeeValidator(IUnitOfWork unitOfWork) {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.")
            .MustAsync(async (id, cancellation) => await unitOfWork.Projects.ProjectExistsAsync(id))
            .WithMessage("Project with given ID does not exist.")
            .When(x => x is { ValidateForDelete: false });

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.")
            .MustAsync(async (id, cancellation) => await unitOfWork.Employees.EmployeeExistsAsync(id))
            .WithMessage("Employee with given ID does not exist.")
            .When(x => x is { ValidateForDelete: false });

        RuleFor(x => x)
            .MustAsync(
                async (model, cancellation) =>
                    !await unitOfWork.ProjectEmployees.ExistsProjectEmployeeAsync(model.ProjectId, model.EmployeeId)
            )
            .WithMessage("This employee is already assigned to this project.")
            .When(x => x is { ValidateForDelete: false });
    }
}