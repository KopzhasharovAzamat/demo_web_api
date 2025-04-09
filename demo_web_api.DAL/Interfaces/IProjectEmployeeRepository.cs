using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Interfaces;

public interface IProjectEmployeeRepository {
    Task AddProjectEmployeeAsync(ProjectEmployee projectEmployee);
    Task AssignEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds);
    Task RemoveProjectEmployeeAsync(Guid projectId, Guid employeeId);
    Task<bool> ExistsProjectEmployeeAsync(Guid projectId, Guid employeeId);
    Task<List<Employee>> GetEmployeesByProjectAsync(Guid projectId);
    Task<List<Project>> GetProjectsByEmployeeAsync(Guid employeeId);
    Task<List<ProjectEmployee>> GetAllProjectEmployeesAsync();
}