using demo_web_api.BLL.Validation.CompanyValidators;
using demo_web_api.BLL.Validation.EmployeeValidators;
using demo_web_api.BLL.Validation.ProjectEmployeeValidators;
using demo_web_api.BLL.Validation.ProjectValidators;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Company;
using demo_web_api.DTOs.Employee;
using demo_web_api.DTOs.Project;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace demo_web_api.BLL.Validation;

public static class ValidatorsExtension {
    public static IServiceCollection AddValidators(this IServiceCollection services) {
        // Employee validators
        services.AddScoped<IValidator<AddEmployeeVm>, AddEmployeeValidator>();
        services.AddScoped<IValidator<UpdateEmployeeVm>, UpdateEmployeeValidator>();

        // Project validators
        services.AddScoped<IValidator<AddProjectVm>, AddProjectValidator>();
        services.AddScoped<IValidator<UpdateProjectVm>, UpdateProjectValidator>();

        // Company
        services.AddScoped<IValidator<AddCompanyVm>, AddCompanyValidator>();
        services.AddScoped<IValidator<UpdateCompanyVm>, UpdateCompanyValidator>();

        // ProjectEmployee
        services.AddScoped<IValidator<ProjectEmployee>, ProjectEmployeeValidator>();

        return services;
    }
}