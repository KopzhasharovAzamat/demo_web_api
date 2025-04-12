using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Employee;
using demo_web_api.DTOs.Project;
using demo_web_api.DTOs.ProjectEmployee;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase {
    private readonly IProjectService _projectService;
    private readonly IMapper         _mapper;

    public ProjectsController(
        IProjectService projectService,
        IMapper         mapper
    ) {
        _projectService = projectService;
        _mapper         = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects() {
        var projects = await _projectService.GetAllProjectsAsync();
        var result   = _mapper.Map<List<ProjectVm>>(projects);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id) {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project is null) return NotFound();

        var result = _mapper.Map<ProjectVm>(project);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectDto projectDto) {
        var newProject = _mapper.Map<Project>(projectDto);
        newProject.Id = Guid.NewGuid();

        await _projectService.AddProjectAsync(newProject);

        return CreatedAtAction(nameof(GetProjectById), new { id = newProject.Id }, projectDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, ProjectDto projectDto) {
        var existingProject = await _projectService.GetProjectByIdAsync(id);
        if (existingProject == null) {
            return NotFound();
        }

        _mapper.Map(projectDto, existingProject);

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