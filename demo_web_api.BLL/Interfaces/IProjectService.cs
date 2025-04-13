using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Project;

namespace demo_web_api.BLL.Interfaces;

public interface IProjectService {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task<Project> AddProjectAsync(AddProjectVm addProjectVm);
    Task<Project> UpdateProjectAsync(Guid id, UpdateProjectVm updateProjectVm);
    Task DeleteProjectAsync(Guid id);
    Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId);
    Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds);
    Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId);
}