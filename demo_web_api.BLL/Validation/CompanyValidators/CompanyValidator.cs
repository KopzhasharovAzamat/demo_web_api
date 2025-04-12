using demo_web_api.DAL.Entities;
using FluentValidation;

namespace demo_web_api.BLL.Validation.CompanyValidators;

public class CompanyValidator : AbstractValidator<Company> {
    public CompanyValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be less 100 characters.");
    }
}