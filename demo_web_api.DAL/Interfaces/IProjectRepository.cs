using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Interfaces;

public interface IProjectRepository {
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(Guid id);
    Task<bool> ProjectExistsAsync(Guid id);
}