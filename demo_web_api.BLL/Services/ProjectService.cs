using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class ProjectService : IProjectService {
    private readonly IUnitOfWork         _unitOfWork;
    private readonly IValidator<Project> _projectValidator;

    public ProjectService(IUnitOfWork unitOfWork, IValidator<Project> projectValidator) {
        _unitOfWork       = unitOfWork;
        _projectValidator = projectValidator;
    }

    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _unitOfWork.Projects.GetAllProjectsAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(Guid id) {
        return await _unitOfWork.Projects.GetProjectByIdAsync(id);
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

    public async Task AddProjectAsync(Project project) {
        var validationResult = await _projectValidator.ValidateAsync(project);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Projects.AddProjectAsync(project);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateProjectAsync(Project project) {
        var validationResult = await _projectValidator.ValidateAsync(project);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Projects.UpdateProjectAsync(project);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteProjectAsync(Guid id) {
        await _unitOfWork.Projects.DeleteProjectAsync(id);
        await _unitOfWork.SaveAsync();
    }
}