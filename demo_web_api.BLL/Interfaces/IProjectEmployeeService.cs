using demo_web_api.DTOs.ProjectEmployee;

namespace demo_web_api.BLL.Interfaces;

public interface IProjectEmployeeService {
    Task AssignEmployeesToProjectAsync(AssignEmployeesDto  dto);
    Task RemoveEmployeeFromProjectAsync(ProjectEmployeeDto dto);
    Task<List<ProjectEmployeeVm>> GetAllProjectEmployeesAsync();
    Task<List<ProjectEmployeeVm>> GetEmployeesByProjectAsync(Guid projectId);
    Task<List<ProjectEmployeeVm>> GetProjectsByEmployeeAsync(Guid employeeId);
}