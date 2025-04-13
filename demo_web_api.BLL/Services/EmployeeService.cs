using System.Net.Http.Headers;
using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.BLL.Validation.EmployeeValidators;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Employee;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class EmployeeService : IEmployeeService {
    private readonly IUnitOfWork                  _unitOfWork;
    private readonly IValidator<AddEmployeeVm>    _addEmployeeValidator;
    private readonly IValidator<UpdateEmployeeVm> _updateEmployeeValidator;
    private readonly IMapper                      _mapper;

    public EmployeeService(
        IUnitOfWork                  unitOfWork,
        IValidator<AddEmployeeVm>    addEmployeeValidator,
        IValidator<UpdateEmployeeVm> updateEmployeeValidator,
        IMapper                      mapper
    ) {
        _unitOfWork              = unitOfWork;
        _addEmployeeValidator    = addEmployeeValidator;
        _updateEmployeeValidator = updateEmployeeValidator;
        _mapper                  = mapper;
    }

    // Get all employees
    public async Task<List<Employee>> GetAllEmployeesAsync() {
        return await _unitOfWork.Employees.GetAllEmployeesAsync();
    }

    // Get employee by id
    public async Task<Employee?> GetEmployeeByIdAsync(Guid id) {
        return await _unitOfWork.Employees.GetEmployeeByIdAsync(id);
    }

    // Get employee by email
    public async Task<Employee?> GetEmployeeByEmailAsync(string email) {
        return await _unitOfWork.Employees.GetEmployeeByEmailAsync(email);
    }

    // Add employee
    public async Task<Employee> AddEmployeeAsync(AddEmployeeVm addEmployeeVm) {
        var validationResult = await _addEmployeeValidator.ValidateAsync(addEmployeeVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var newEmployee = _mapper.Map<Employee>(addEmployeeVm);
        newEmployee.Id = Guid.NewGuid();

        _unitOfWork.Employees.AddEmployeeAsync(newEmployee);
        await _unitOfWork.SaveAsync();

        return newEmployee;
    }

    // Update employee
    public async Task<Employee> UpdateEmployeeAsync(Guid id, UpdateEmployeeVm updateEmployeeVm) {
        var validationResult = await _updateEmployeeValidator.ValidateAsync(updateEmployeeVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var existingEmployee = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);
        if (existingEmployee is null) {
            throw new("Employee not found");
        }

        _mapper.Map(updateEmployeeVm, existingEmployee);

        _unitOfWork.Employees.UpdateEmployeeAsync(existingEmployee);
        await _unitOfWork.SaveAsync();

        return existingEmployee;
    }

    // Delete employee
    public async Task DeleteEmployeeAsync(Guid id) {
        await _unitOfWork.Employees.DeleteEmployeeAsync(id);
        await _unitOfWork.SaveAsync();
    }
}