using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs.Project;
using demo_web_api.DTOs.ProjectEmployee;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase {
    private readonly IProjectService _projectService;

    public ProjectsController(
        IProjectService projectService
    ) {
        _projectService = projectService;
    }

    // Get all projects
    [HttpGet]
    public async Task<IActionResult> GetAllProjects() {
        var projects = await _projectService.GetAllProjectsAsync();

        return Ok(projects);
    }

    // Get project by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id) {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project is null) return NotFound();

        return Ok(project);
    }

    // Create project
    [HttpPost]
    public async Task<IActionResult> CreateProject(AddProjectVm addProjectVm) {
        var newProject = await _projectService.AddProjectAsync(addProjectVm);

        return CreatedAtAction(nameof(GetProjectById), new { id = newProject.Id }, newProject);
    }

    // Update project
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectVm updateProjectVm) {
        var updatedProject = await _projectService.UpdateProjectAsync(id, updateProjectVm);

        return Ok(updatedProject);
    }

    // Delete project
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id) {
        await _projectService.DeleteProjectAsync(id);

        return NoContent();
    }

    // Get list of employees working on a project by project id
    [HttpGet("{projectId:guid}/employees")]
    public async Task<IActionResult> GetEmployeesByProject(Guid projectId) {
        var employees = await _projectService.GetEmployeesByProjectIdAsync(projectId);
        
        return Ok(employees);
    }

    // Add a list of employee to a project
    [HttpPost("{projectId:guid}/employees")]
    public async Task<IActionResult> AddEmployeesToProject(Guid projectId, AssignEmployeesDto dto) {
        await _projectService.AddEmployeesToProjectAsync(projectId, dto.EmployeeIds);

        return NoContent();
    }

    // Remove employee from project
    [HttpDelete("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> RemoveEmployeeFromProject(Guid projectId, Guid employeeId) {
        await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);

        return NoContent();
    }
}