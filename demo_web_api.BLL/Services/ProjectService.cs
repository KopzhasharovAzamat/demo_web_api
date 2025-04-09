using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;

namespace demo_web_api.BLL.Services;

public class ProjectService : IProjectService {
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _unitOfWork.Projects.GetAllProjectsAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(Guid id) {
        return await _unitOfWork.Projects.GetProjectByIdAsync(id);
    }

    public async Task AddProjectAsync(Project project) {
        await _unitOfWork.Projects.AddProjectAsync(project);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateProjectAsync(Project project) {
        await _unitOfWork.Projects.UpdateProjectAsync(project);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteProjectAsync(Guid id) {
        await _unitOfWork.Projects.DeleteProjectAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId) {
        return await _unitOfWork.Projects.GetEmployeesByProjectIdAsync(projectId);
    }

    public async Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds) {
        await _unitOfWork.Projects.AddEmployeesToProjectAsync(projectId, employeeIds);
        await _unitOfWork.SaveAsync();
    }

    public async Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId) {
        await _unitOfWork.Projects.RemoveEmployeeFromProjectAsync(projectId, employeeId);
        await _unitOfWork.SaveAsync();
    }
}