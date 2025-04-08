using demo_web_api.DAL.Entities;

namespace demo_web_api.BLL.Interfaces;

public interface IProjectEmployeeService {
    Task AssignEmployeeToProjectAsync(Guid projectId, Guid employeeId);
    Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId);
    Task<List<Employee>> GetEmployeesByProjectAsync(Guid projectId);
    Task<List<Project>> GetProjectsByEmployeeAsync(Guid employeeId);
    Task<List<ProjectEmployee>> GetAllProjectEmployeesAsync();
}