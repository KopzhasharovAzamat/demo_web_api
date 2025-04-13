using demo_web_api.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace demo_web_api.BLL.Services;

public static class ServicesExtension {
    public static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();

        return services;
    }
}