using demo_web_api.DTOs.Company;
using FluentValidation;

namespace demo_web_api.BLL.Validation.CompanyValidators;

public class AddCompanyValidator : AbstractValidator<AddCompanyVm> {
    public AddCompanyValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be less than 100 characters.");
    }
}