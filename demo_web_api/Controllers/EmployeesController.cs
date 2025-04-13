using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Employee;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase {
    private readonly IEmployeeService _employeeService;

    public EmployeesController(
        IEmployeeService employeeService
    ) {
        _employeeService = employeeService;
    }

    // Get all employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees() {
        var employees = await _employeeService.GetAllEmployeesAsync();

        return Ok(employees);
    }

    // Get employee by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid id) {
        var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);

        if (existingEmployee is null) return NotFound();

        return Ok(existingEmployee);
    }

    // Create employee
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(AddEmployeeVm addEmployeeVm) {
        var created = await _employeeService.AddEmployeeAsync(addEmployeeVm);

        return CreatedAtAction(nameof(GetEmployeeById), new { id = created.Id }, created);
    }

    // Update employee
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeVm updateEmployeeVm) {
        var updated = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeVm);

        return Ok(updated);
    }

    // Delete employee
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id) {
        await _employeeService.DeleteEmployeeAsync(id);

        return NoContent();
    }
}