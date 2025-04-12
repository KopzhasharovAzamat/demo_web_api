using AutoMapper;
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
    private readonly IMapper                 _mapper;

    public EmployeesController(
        IEmployeeService        employeeService,
        IValidator<EmployeeDto> employeeValidator,
        IMapper                 mapper
    ) {
        _employeeService   = employeeService;
        _employeeValidator = employeeValidator;
        _mapper            = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees() {
        var employees = await _employeeService.GetAllEmployeesAsync();
        var result    = _mapper.Map<List<EmployeeVm>>(employees);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid id) {
        var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);

        if (existingEmployee is null) return NotFound();

        var result = _mapper.Map<EmployeeVm>(existingEmployee);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto) {
        var validationResult = await _employeeValidator.ValidateAsync(employeeDto);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors);
        }

        var newEmployee = _mapper.Map<Employee>(employeeDto);
        newEmployee.Id = Guid.NewGuid();

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

        _mapper.Map(employeeDto, existingEmployee);

        await _employeeService.UpdateEmployeeAsync(existingEmployee);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id) {
        await _employeeService.DeleteEmployeeAsync(id);

        return NoContent();
    }
}