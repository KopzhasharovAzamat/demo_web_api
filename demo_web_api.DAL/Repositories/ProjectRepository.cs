using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories;

public class ProjectRepository : IProjectRepository {
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    public async Task<List<Project>> GetAllProjectsAsync() {
        return await _dbContext.Projects
            .Include(x => x.CustomerCompany)
            .Include(x => x.ContractorCompany)
            .Include(x => x.ProjectManager)
            .Include(x => x.ProjectEmployees)
            .ThenInclude(x => x.Employee)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(Guid id) {
        return await _dbContext.Projects
            .Include(x => x.ProjectEmployees)
            .ThenInclude(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddProjectAsync(Project project) {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(Project project) {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid id) {
        var project = await _dbContext.Projects.FindAsync(id);
        if (project != null) {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ProjectExistsAsync(Guid id) {
        return await _dbContext.Projects.AnyAsync(x => x.Id == id);
    }
}