using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Employee;
using demo_web_api.DTOs.Project;
using demo_web_api.DTOs.ProjectEmployee;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase {
    private readonly IProjectService        _projectService;
    private readonly IValidator<ProjectDto> _projectValidator;

    public ProjectsController(IProjectService projectService, IValidator<ProjectDto> projectValidator) {
        _projectService   = projectService;
        _projectValidator = projectValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects() {
        var projects = await _projectService.GetAllProjectsAsync();
        var result = projects.Select(
            project => new ProjectVm {
                Id                    = project.Id,
                Name                  = project.Name,
                CustomerCompanyId     = project.CustomerCompanyId,
                CustomerCompanyName   = project.CustomerCompany?.Name,
                ContractorCompanyId   = project.ContractorCompanyId,
                ContractorCompanyName = project.ContractorCompany?.Name,
                StartDate             = project.StartDate,
                EndDate               = project.EndDate,
                Priority              = project.Priority,
                ProjectManagerId      = project.ProjectManagerId,
                ProjectManagerName = project.ProjectManager != null ?
                    $"{project.ProjectManager.LastName} {project.ProjectManager.FirstName}" :
                    null
            }
        );

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id) {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project is null) return NotFound();

        var foundProject = new ProjectVm {
            Id                  = project.Id,
            Name                = project.Name,
            CustomerCompanyId   = project.CustomerCompanyId,
            ContractorCompanyId = project.ContractorCompanyId,
            StartDate           = project.StartDate,
            EndDate             = project.EndDate,
            Priority            = project.Priority,
            ProjectManagerId    = project.ProjectManagerId
        };

        return Ok(foundProject);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectDto projectDto) {
        var validationResult = await _projectValidator.ValidateAsync(projectDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        var newProject = new Project {
            Id                  = Guid.NewGuid(),
            Name                = projectDto.Name,
            CustomerCompanyId   = projectDto.CustomerCompanyId,
            ContractorCompanyId = projectDto.ContractorCompanyId,
            StartDate           = projectDto.StartDate,
            EndDate             = projectDto.EndDate,
            Priority            = projectDto.Priority,
            ProjectManagerId    = projectDto.ProjectManagerId
        };

        await _projectService.AddProjectAsync(newProject);

        return CreatedAtAction(nameof(GetProjectById), new { id = newProject.Id }, projectDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, ProjectDto projectDto) {
        var validationResult = await _projectValidator.ValidateAsync(projectDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        var existingProject = await _projectService.GetProjectByIdAsync(id);
        if (existingProject == null) {
            return NotFound();
        }

        existingProject.Name                = projectDto.Name;
        existingProject.CustomerCompanyId   = projectDto.CustomerCompanyId;
        existingProject.ContractorCompanyId = projectDto.ContractorCompanyId;
        existingProject.StartDate           = projectDto.StartDate;
        existingProject.EndDate             = projectDto.EndDate;
        existingProject.Priority            = projectDto.Priority;
        existingProject.ProjectManagerId    = projectDto.ProjectManagerId;

        await _projectService.UpdateProjectAsync(existingProject);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id) {
        await _projectService.DeleteProjectAsync(id);

        return NoContent();
    }

    [HttpGet("{projectId:guid}/employees")]
    public async Task<IActionResult> GetEmployeesByProject(Guid projectId) {
        var employees = await _projectService.GetEmployeesByProjectIdAsync(projectId);
        var result = employees.Select(
            e => new EmployeeVm {
                Id         = e.Id,
                LastName   = e.LastName,
                FirstName  = e.FirstName,
                MiddleName = e.MiddleName,
                Email      = e.Email
            }
        );

        return Ok(result);
    }

    [HttpPost("{projectId:guid}/employees")]
    public async Task<IActionResult> AddEmployeesToProject(Guid projectId, AssignEmployeesDto dto) {
        await _projectService.AddEmployeesToProjectAsync(projectId, dto.EmployeeIds);

        return NoContent();
    }

    [HttpDelete("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> RemoveEmployeeFromProject(Guid projectId, Guid employeeId) {
        await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);

        return NoContent();
    }
}