using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs;
using demo_web_api.Validation;
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

    [HttpPost("assign")]
    public async Task<IActionResult> AssignEmployee(ProjectEmployeeDto projectEmployeeDto) {
        var validationResult = await _projectEmployeeValidator.ValidateAsync(projectEmployeeDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        await _projectEmployeeService.AssignEmployeeToProjectAsync(
            projectEmployeeDto.ProjectId,
            projectEmployeeDto.EmployeeId
        );

        return Ok("Employee assigned to project.");
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveEmployee(ProjectEmployeeDto projectEmployeeDto) {
        var validationResult = await _projectEmployeeValidator.ValidateAsync(projectEmployeeDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        await _projectEmployeeService.RemoveEmployeeFromProjectAsync(
            projectEmployeeDto.ProjectId,
            projectEmployeeDto.EmployeeId
        );

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