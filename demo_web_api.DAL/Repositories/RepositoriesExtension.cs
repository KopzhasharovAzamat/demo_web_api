using demo_web_api.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace demo_web_api.DAL.Repositories;

public static class RepositoriesExtension {
    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();

        return services;
    }
}