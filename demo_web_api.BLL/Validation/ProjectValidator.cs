using demo_web_api.DAL.Entities;
using FluentValidation;

namespace demo_web_api.BLL.Validation;

public class ProjectValidator : AbstractValidator<Project> {
    public ProjectValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(100).WithMessage("Project name must be less 100 characters.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate ?? DateTime.MaxValue)
            .WithMessage("Start date must be earlier than or equal to end date.");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5)
            .WithMessage("Priority must be between 1 and 5.");
    }
}