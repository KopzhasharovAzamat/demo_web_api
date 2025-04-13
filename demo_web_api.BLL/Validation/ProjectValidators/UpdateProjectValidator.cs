using demo_web_api.DTOs.Project;
using FluentValidation;

namespace demo_web_api.BLL.Validation.ProjectValidators;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectVm> {
    public UpdateProjectValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(100).WithMessage("Project name must be less 100 characters.");

        RuleFor(x => x.ContractorCompanyId)
            .NotEmpty().WithMessage("Contractor company is required.");

        RuleFor(x => x.CustomerCompanyId)
            .NotEmpty().WithMessage("Customer company is required.");

        RuleFor(x => x.ProjectManagerId)
            .NotEmpty().WithMessage("Project manager is required.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate ?? DateTime.MaxValue)
            .WithMessage("Start date must be earlier than or equal to end date.");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5)
            .WithMessage("Priority must be between 1 and 5.");
    }
}