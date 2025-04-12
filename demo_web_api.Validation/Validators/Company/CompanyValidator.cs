using demo_web_api.DTOs.Company;
using FluentValidation;

namespace demo_web_api.Validation.Validators.Company;

public class CompanyValidator : AbstractValidator<CompanyDto> {
    public CompanyValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be less 100 characters.");
    }
}