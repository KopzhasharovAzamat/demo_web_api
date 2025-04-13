using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Project;
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
            .ThenInclude(pe => pe.Employee)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Project>> GetFilteredProjectsAsync(ProjectQueryParameters parameters) {
        var query = _dbContext.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ContractorCompany)
            .Include(p => p.ProjectManager)
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employee)
            .AsQueryable();

        if (parameters.StartDateFrom.HasValue) {
            query = query.Where(p => p.StartDate >= parameters.StartDateFrom.Value);
        }

        if (parameters.StartDateTo.HasValue) {
            query = query.Where(p => p.StartDate <= parameters.StartDateTo.Value);
        }

        if (parameters.MinPriority.HasValue) {
            query = query.Where(p => p.Priority >= parameters.MinPriority.Value);
        }

        if (parameters.MaxPriority.HasValue) {
            query = query.Where(p => p.Priority <= parameters.MaxPriority.Value);
        }

        query = parameters.SortBy?.ToLower() switch {
            "name" => parameters.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "startdate" => parameters.Descending ?
                query.OrderByDescending(p => p.StartDate) :
                query.OrderBy(p => p.StartDate),
            "enddate" => parameters.Descending ?
                query.OrderByDescending(p => p.EndDate) :
                query.OrderBy(p => p.EndDate),
            "priority" => parameters.Descending ?
                query.OrderByDescending(p => p.Priority) :
                query.OrderBy(p => p.Priority),
            _ => query.OrderBy(p => p.Name) // default
        };

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(Guid id) {
        return await _dbContext.Projects
            .Include(x => x.ProjectEmployees)
            .ThenInclude(x => x.Employee)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Employee>> GetEmployeesByProjectIdAsync(Guid projectId) {
        return await _dbContext.ProjectEmployees
            .Where(pe => pe.ProjectId == projectId)
            .Include(pe => pe.Employee)
            .Select(pe => pe.Employee)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds) {
        var existingLinks = await _dbContext.ProjectEmployees
            .Where(pe => pe.ProjectId == projectId)
            .Select(pe => pe.EmployeeId)
            .ToListAsync();

        var newLinks = employeeIds
            .Except(existingLinks)
            .Select(
                eId => new ProjectEmployee {
                    ProjectId  = projectId,
                    EmployeeId = eId
                }
            );

        _dbContext.ProjectEmployees.AddRange(newLinks);
    }

    public async Task RemoveEmployeeFromProjectAsync(Guid projectId, Guid employeeId) {
        var link = await _dbContext.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

        if (link != null) {
            _dbContext.ProjectEmployees.Remove(link);
        }
    }

    public async Task<bool> ProjectExistsAsync(Guid id) {
        return await _dbContext.Projects.AnyAsync(x => x.Id == id);
    }

    public void AddProjectAsync(Project project) {
        _dbContext.Projects.Add(project);
    }

    public void UpdateProjectAsync(Project project) {
        _dbContext.Projects.Update(project);
    }

    public async Task DeleteProjectAsync(Guid id) {
        var project = await _dbContext.Projects.FindAsync(id);
        if (project != null) {
            _dbContext.Projects.Remove(project);
        }
    }
}