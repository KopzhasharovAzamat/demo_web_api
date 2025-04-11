using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs.Employee;
using FluentValidation;

namespace demo_web_api.Validation;

public class EmployeeValidator : AbstractValidator<EmployeeDto> {
    public EmployeeValidator(IEmployeeService employeeService) {
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must be less 100 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must be less 100 characters.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Company name must be less 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.")
            .MustAsync(
                async (email, cancellation) => {
                    var existing = await employeeService.GetEmployeeByEmailAsync(email);

                    return existing is null;
                }
            ).WithMessage("Email must be unique.");
    }
}