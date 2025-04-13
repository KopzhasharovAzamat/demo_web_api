using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories;

public class ProjectEmployeeRepository : IProjectEmployeeRepository {
    private readonly ApplicationDbContext _dbContext;

    public ProjectEmployeeRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    // Get all ProjectEmployee entities
    public async Task<List<ProjectEmployee>> GetAllProjectEmployeesAsync() {
        return await _dbContext.ProjectEmployees
            .Include(pe => pe.Employee)
            .Include(pe => pe.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    // Get list of employees by project
    public async Task<List<Employee>> GetEmployeesByProjectAsync(Guid projectId) {
        return await _dbContext.ProjectEmployees
            .Where(pe => pe.ProjectId == projectId)
            .Select(pe => pe.Employee)
            .AsNoTracking()
            .ToListAsync();
    }

    // Get list of project by employee
    public async Task<List<Project>> GetProjectsByEmployeeAsync(Guid employeeId) {
        return await _dbContext.ProjectEmployees
            .Where(pe => pe.EmployeeId == employeeId)
            .Include(pe => pe.Project)
            .ThenInclude(p => p.CustomerCompany)
            .Include(pe => pe.Project)
            .ThenInclude(p => p.ContractorCompany)
            .Include(pe => pe.Project)
            .ThenInclude(p => p.ProjectManager)
            .Select(pe => pe.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    // Add ProjectEmployee entity
    public void AddProjectEmployeeAsync(ProjectEmployee projectEmployee) {
        _dbContext.ProjectEmployees.Add(projectEmployee);
    }

    // Remove EmployeeProject entity
    public async Task RemoveProjectEmployeeAsync(Guid projectId, Guid employeeId) {
        var entity = await _dbContext.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
        if (entity is not null) {
            _dbContext.ProjectEmployees.Remove(entity);
        }
    }

    public async Task AssignEmployeesToProjectAsync(Guid projectId, List<Guid> employeeIds) {
        foreach (var employeeId in employeeIds) {
            var exists = await _dbContext.ProjectEmployees
                .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (!exists) {
                var projectEmployee = new ProjectEmployee {
                    ProjectId  = projectId,
                    EmployeeId = employeeId
                };

                _dbContext.ProjectEmployees.Add(projectEmployee);
            }
        }
    }

    public async Task<bool> ExistsProjectEmployeeAsync(Guid projectId, Guid employeeId) {
        return await _dbContext.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }
}