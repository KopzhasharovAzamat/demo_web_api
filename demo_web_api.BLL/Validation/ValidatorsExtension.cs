using demo_web_api.BLL.Validation.CompanyValidators;
using demo_web_api.BLL.Validation.EmployeeValidators;
using demo_web_api.BLL.Validation.ProjectEmployeeValidators;
using demo_web_api.BLL.Validation.ProjectValidators;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Company;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace demo_web_api.BLL.Validation;

public static class ValidatorsExtension {
    public static IServiceCollection AddValidators(this IServiceCollection services) {
        // Employee validators
        services.AddScoped<IValidator<Employee>, EmployeeValidator>();
        
        // Project validators
        services.AddScoped<IValidator<Project>, ProjectValidator>();
        
        // Company
        services.AddScoped<IValidator<AddCompanyVm>, AddCompanyValidator>();
        services.AddScoped<IValidator<UpdateCompanyVm>, UpdateCompanyValidator>();
        services.AddScoped<IValidator<Company>, CompanyValidator>();

        // ProjectEmployee
        services.AddScoped<IValidator<ProjectEmployee>, ProjectEmployeeValidator>();

        return services;
    }
}