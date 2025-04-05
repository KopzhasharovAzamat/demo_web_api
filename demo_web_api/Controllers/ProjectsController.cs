using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase {
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService) {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects() {
        var projects = await _projectService.GetAllProjectsAsync();
        var result = projects.Select(
            project => new ProjectDto {
                Id                  = project.Id,
                Name                = project.Name,
                CustomerCompanyId   = project.CustomerCompanyId,
                ContractorCompanyId = project.ContractorCompanyId,
                StartDate           = project.StartDate,
                EndDate             = project.EndDate,
                Priority            = project.Priority,
                ProjectManagerId    = project.ProjectManagerId
            }
        );

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id) {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project is null) return NotFound();

        var foundProject = new ProjectDto {
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
    public async Task<IActionResult> CreateProject(ProjectDto dto) {
        var newProject = new Project {
            Id                  = Guid.NewGuid(),
            Name                = dto.Name,
            CustomerCompanyId   = dto.CustomerCompanyId,
            ContractorCompanyId = dto.ContractorCompanyId,
            StartDate           = dto.StartDate,
            EndDate             = dto.EndDate,
            Priority            = dto.Priority,
            ProjectManagerId    = dto.ProjectManagerId
        };

        await _projectService.AddProjectAsync(newProject);

        return CreatedAtAction(nameof(GetProjectById), new { id = newProject.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, ProjectDto projectDto) {
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
}