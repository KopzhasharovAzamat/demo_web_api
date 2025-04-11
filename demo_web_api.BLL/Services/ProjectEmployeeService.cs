using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;

namespace demo_web_api.BLL.Services;

public class ProjectEmployeeService : IProjectEmployeeService {
    private readonly IUnitOfWork _unitOfWork;

    public ProjectEmployeeService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public async Task AssignEmployeeToProjectAsync(Guid projectId, Guid employeeId) {
        var exists = await _unitOfWork.ProjectEmployees.ExistsProjectEmployeeAsync(projectId, employeeId);
        if (!exists) {
            var entity = new ProjectEmployee {
                ProjectId  = projectId,
                EmployeeId = employeeId
            };

            _unitOfWork.ProjectEmployees.AddProjectEmployeeAsync(entity);
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task AssignEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds) {
        await _unitOfWork.ProjectEmployees.AssignEmployeesToProjectAsync(projectId, employeeIds);
        await _unitOfWork.SaveAsync();
    }

    public async Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId) {
        await _unitOfWork.ProjectEmployees.RemoveProjectEmployeeAsync(projectId, employeeId);
    }

    public async Task<List<Employee>> GetEmployeesByProjectAsync(Guid projectId) {
        return await _unitOfWork.ProjectEmployees.GetEmployeesByProjectAsync(projectId);
    }

    public async Task<List<Project>> GetProjectsByEmployeeAsync(Guid employeeId) {
        return await _unitOfWork.ProjectEmployees.GetProjectsByEmployeeAsync(employeeId);
    }

    public async Task<List<ProjectEmployee>> GetAllProjectEmployeesAsync() {
        return await _unitOfWork.ProjectEmployees.GetAllProjectEmployeesAsync();
    }
}