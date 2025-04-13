using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Project;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class ProjectService : IProjectService {
    private readonly IUnitOfWork                 _unitOfWork;
    private readonly IMapper                     _mapper;
    private readonly IValidator<AddProjectVm>    _addValidator;
    private readonly IValidator<UpdateProjectVm> _updateValidator;

    public ProjectService(
        IUnitOfWork                 unitOfWork,
        IMapper                     mapper,
        IValidator<AddProjectVm>    addValidator,
        IValidator<UpdateProjectVm> updateValidator
    ) {
        _unitOfWork      = unitOfWork;
        _mapper          = mapper;
        _addValidator    = addValidator;
        _updateValidator = updateValidator;
    }

    // Get all projects
    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _unitOfWork.Projects.GetAllProjectsAsync();
    }

    // Get project by id
    public async Task<Project?> GetProjectByIdAsync(Guid id) {
        return await _unitOfWork.Projects.GetProjectByIdAsync(id);
    }

    // Get employees working on a project by project id
    public async Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId) {
        return await _unitOfWork.Projects.GetEmployeesByProjectIdAsync(projectId);
    }

    // Add list of employees to project by project id
    public async Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds) {
        await _unitOfWork.Projects.AddEmployeesToProjectAsync(projectId, employeeIds);
        await _unitOfWork.SaveAsync();
    }

    // Delete employee from project by project id
    public async Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId) {
        await _unitOfWork.Projects.RemoveEmployeeFromProjectAsync(projectId, employeeId);
        await _unitOfWork.SaveAsync();
    }

    // Add project
    public async Task<Project> AddProjectAsync(AddProjectVm addProjectVm) {
        var validationResult = await _addValidator.ValidateAsync(addProjectVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var newProject = _mapper.Map<Project>(addProjectVm);
        newProject.Id = Guid.NewGuid();

        _unitOfWork.Projects.AddProjectAsync(newProject);
        await _unitOfWork.SaveAsync();

        return newProject;
    }

    // Update project
    public async Task<Project> UpdateProjectAsync(Guid id, UpdateProjectVm updateProjectVm) {
        var validationResult = await _updateValidator.ValidateAsync(updateProjectVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var existingProject = await _unitOfWork.Projects.GetProjectByIdAsync(id);
        if (existingProject is null) {
            throw new("Project not found");
        }

        _mapper.Map(updateProjectVm, existingProject);

        _unitOfWork.Projects.UpdateProjectAsync(existingProject);
        await _unitOfWork.SaveAsync();

        return existingProject;
    }

    // Delete project
    public async Task DeleteProjectAsync(Guid id) {
        await _unitOfWork.Projects.DeleteProjectAsync(id);
        await _unitOfWork.SaveAsync();
    }
}