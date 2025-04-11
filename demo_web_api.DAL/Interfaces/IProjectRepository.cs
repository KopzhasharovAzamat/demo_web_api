using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Interfaces;

public interface IProjectRepository {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId);
    Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds);
    Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId);
    Task<bool> ProjectExistsAsync(Guid id);
    void AddProjectAsync(Project project);
    void UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(Guid id);
}