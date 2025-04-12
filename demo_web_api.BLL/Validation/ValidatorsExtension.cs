using demo_web_api.BLL.Validation.CompanyValidators;
using demo_web_api.BLL.Validation.EmployeeValidators;
using demo_web_api.BLL.Validation.ProjectEmployeeValidators;
using demo_web_api.BLL.Validation.ProjectValidators;
using demo_web_api.DAL.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace demo_web_api.BLL.Validation;

public static class ValidatorsExtension {
    public static IServiceCollection AddValidators(this IServiceCollection services) {
        services.AddScoped<IValidator<Employee>, EmployeeValidator>();
        services.AddScoped<IValidator<Project>, ProjectValidator>();
        services.AddScoped<IValidator<Company>, CompanyValidator>();
        services.AddScoped<IValidator<ProjectEmployee>, ProjectEmployeeValidator>();

        return services;
    }
}