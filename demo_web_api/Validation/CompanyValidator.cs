using demo_web_api.ViewModels;
using FluentValidation;

namespace demo_web_api.Validation;

public class CompanyValidator : AbstractValidator<CompanyDto> {
    public CompanyValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be less 100 characters.");
    }
}