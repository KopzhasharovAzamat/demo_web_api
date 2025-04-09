using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs;
using demo_web_api.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectEmployeeController : ControllerBase {
    private readonly IProjectEmployeeService        _projectEmployeeService;
    private readonly IValidator<ProjectEmployeeDto> _projectEmployeeValidator;

    public ProjectEmployeeController(
        IProjectEmployeeService        projectEmployeeService,
        IValidator<ProjectEmployeeDto> projectEmployeeValidator
    ) {
        _projectEmployeeService   = projectEmployeeService;
        _projectEmployeeValidator = projectEmployeeValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjectEmployees() {
        var projectEmployees = await _projectEmployeeService.GetAllProjectEmployeesAsync();

        var result = projectEmployees.Select(
            pe => new ProjectEmployeeVm() {
                EmployeeId  = pe.Employee.Id,
                FullName    = $"{pe.Employee.LastName} {pe.Employee.FirstName} {pe.Employee.MiddleName}",
                Email       = pe.Employee.Email,
                ProjectId   = pe.Project.Id,
                ProjectName = pe.Project.Name,
                StartDate   = pe.Project.StartDate,
                EndDate     = pe.Project.EndDate
            }
        ).ToList();

        return Ok(result);
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
        var validationResult = await _projectEmployeeValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        await _projectEmployeeService.RemoveEmployeeFromProjectAsync(dto.ProjectId, dto.EmployeeId);

        return Ok("Employee removed from project.");
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetEmployeesByProject(Guid projectId) {
        var result = await _projectEmployeeService.GetEmployeesByProjectAsync(projectId);

        return Ok(
            result.Select(
                e => new {
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.Email
                }
            )
        );
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetProjectsByEmployee(Guid employeeId) {
        var result = await _projectEmployeeService.GetProjectsByEmployeeAsync(employeeId);

        return Ok(
            result.Select(
                p => new {
                    p.Id,
                    p.Name,
                    p.StartDate,
                    p.EndDate,
                    p.Priority
                }
            )
        );
    }
}