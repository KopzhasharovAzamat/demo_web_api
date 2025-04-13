using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.ProjectEmployee;
using FluentValidation;

namespace demo_web_api.BLL.Validation.ProjectEmployeeValidators;

public class ProjectEmployeeDtoValidator : AbstractValidator<ProjectEmployeeDto> {
    public ProjectEmployeeDtoValidator(IUnitOfWork unitOfWork) {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.")
            .MustAsync(async (id, _) => await unitOfWork.Projects.ProjectExistsAsync(id))
            .WithMessage("Project with given ID does not exist.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.")
            .MustAsync(async (id, _) => await unitOfWork.Employees.EmployeeExistsAsync(id))
            .WithMessage("Employee with given ID does not exist.");

        When(
            x => !x.ValidateForDelete,
            () => {
                RuleFor(x => x)
                    .MustAsync(
                        async (model, _) =>
                            !await unitOfWork.ProjectEmployees.ExistsProjectEmployeeAsync(
                                model.ProjectId,
                                model.EmployeeId
                            )
                    )
                    .WithMessage("This employee is already assigned to this project.");
            }
        );

        When(
            x => x.ValidateForDelete,
            () => {
                RuleFor(x => x)
                    .MustAsync(
                        async (model, _) =>
                            await unitOfWork.ProjectEmployees.ExistsProjectEmployeeAsync(
                                model.ProjectId,
                                model.EmployeeId
                            )
                    )
                    .WithMessage("This employee is not assigned to this project.");
            }
        );
    }
}