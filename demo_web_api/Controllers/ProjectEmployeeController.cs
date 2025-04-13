using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs.ProjectEmployee;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectEmployeeController : ControllerBase {
    private readonly IProjectEmployeeService _service;

    public ProjectEmployeeController(IProjectEmployeeService service) {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjectEmployees() {
        var result = await _service.GetAllProjectEmployeesAsync();

        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetEmployeesByProject(Guid projectId) {
        var result = await _service.GetEmployeesByProjectAsync(projectId);

        return Ok(result);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetProjectsByEmployee(Guid employeeId) {
        var result = await _service.GetProjectsByEmployeeAsync(employeeId);

        return Ok(result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignEmployees([FromBody] AssignEmployeesDto dto) {
        await _service.AssignEmployeesToProjectAsync(dto);

        return Ok("Employees assigned.");
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveEmployee([FromBody] ProjectEmployeeDto dto) {
        await _service.RemoveEmployeeFromProjectAsync(dto);

        return Ok("Employee removed.");
    }
}