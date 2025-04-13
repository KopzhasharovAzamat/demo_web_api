using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs.ProjectEmployee;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectEmployeeController : ControllerBase {
    private readonly IProjectEmployeeService _projectEmployeeService;
    private readonly IMapper                 _mapper;

    public ProjectEmployeeController(
        IProjectEmployeeService projectEmployeeService,
        IMapper                 mapper
    ) {
        _projectEmployeeService = projectEmployeeService;
        _mapper                 = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjectEmployees() {
        var projectEmployees = await _projectEmployeeService.GetAllProjectEmployeesAsync();
        var result           = _mapper.Map<List<ProjectEmployeeVm>>(projectEmployees);

        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetEmployeesByProject(Guid projectId) {
        var employeesByProject = await _projectEmployeeService.GetEmployeesByProjectAsync(projectId);

        return Ok(employeesByProject);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetProjectsByEmployee(Guid employeeId) {
        var projectsByEmployee = await _projectEmployeeService.GetProjectsByEmployeeAsync(employeeId);
        
        return Ok(projectsByEmployee);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignEmployees(AssignEmployeesDto dto) {
        if (dto.EmployeeIds == null || dto.EmployeeIds.Count == 0) {
            return BadRequest("Employee list is empty");
        }

        await _projectEmployeeService.AssignEmployeesToProjectAsync(dto.ProjectId, dto.EmployeeIds);

        return Ok();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveEmployee(ProjectEmployeeDto dto) {
        await _projectEmployeeService.RemoveEmployeeFromProjectAsync(dto.ProjectId, dto.EmployeeId);

        return Ok("Employee removed from project.");
    }
}