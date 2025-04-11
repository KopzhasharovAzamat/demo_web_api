using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Employee;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase {
    private readonly IEmployeeService        _employeeService;
    private readonly IValidator<EmployeeDto> _employeeValidator;

    public EmployeesController(IEmployeeService employeeService, IValidator<EmployeeDto> employeeValidator) {
        _employeeService   = employeeService;
        _employeeValidator = employeeValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees() {
        var employees = await _employeeService.GetAllEmployeesAsync();
        var result = employees.Select(
            employee => new EmployeeVm {
                Id         = employee.Id,
                FirstName  = employee.FirstName,
                LastName   = employee.LastName,
                MiddleName = employee.MiddleName,
                Email      = employee.Email
            }
        );

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid id) {
        var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);

        if (existingEmployee is null) return NotFound();

        var foundEmployee = new EmployeeVm() {
            Id         = existingEmployee.Id,
            FirstName  = existingEmployee.FirstName,
            LastName   = existingEmployee.LastName,
            MiddleName = existingEmployee.MiddleName,
            Email      = existingEmployee.Email
        };

        return Ok(foundEmployee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto) {
        var validationResult = await _employeeValidator.ValidateAsync(employeeDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        var newEmployee = new Employee {
            Id         = Guid.NewGuid(),
            LastName   = employeeDto.LastName,
            FirstName  = employeeDto.FirstName,
            MiddleName = employeeDto.MiddleName,
            Email      = employeeDto.Email
        };
        await _employeeService.AddEmployeeAsync(newEmployee);

        return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, employeeDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, EmployeeDto employeeDto) {
        var validationResult = await _employeeValidator.ValidateAsync(employeeDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);
        if (existingEmployee is null) {
            return NotFound();
        }

        existingEmployee.LastName   = employeeDto.LastName;
        existingEmployee.FirstName  = employeeDto.FirstName;
        existingEmployee.MiddleName = employeeDto.MiddleName;
        existingEmployee.Email      = employeeDto.Email;

        await _employeeService.UpdateEmployeeAsync(existingEmployee);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id) {
        await _employeeService.DeleteEmployeeAsync(id);

        return NoContent();
    }
}