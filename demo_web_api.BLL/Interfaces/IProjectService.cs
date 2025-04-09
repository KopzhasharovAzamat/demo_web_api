using demo_web_api.DAL.Entities;

namespace demo_web_api.BLL.Interfaces;

public interface IProjectService {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(Guid id);
    Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId);
    Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds);
    Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId);
}