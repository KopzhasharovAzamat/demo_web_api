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
    }

    public async Task UpdateProjectAsync(Project project) {
        await _unitOfWork.Projects.UpdateProjectAsync(project);
    }

    public async Task DeleteProjectAsync(Guid id) {
        await _unitOfWork.Projects.DeleteProjectAsync(id);
    }
}