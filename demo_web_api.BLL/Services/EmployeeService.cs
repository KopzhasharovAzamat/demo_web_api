using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class EmployeeService : IEmployeeService {
    private readonly IUnitOfWork          _unitOfWork;
    private readonly IValidator<Employee> _employeeValidator;

    public EmployeeService(IUnitOfWork unitOfWork, IValidator<Employee> employeeValidator) {
        _unitOfWork        = unitOfWork;
        _employeeValidator = employeeValidator;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync() {
        return await _unitOfWork.Employees.GetAllEmployeesAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id) {
        return await _unitOfWork.Employees.GetEmployeeByIdAsync(id);
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email) {
        return await _unitOfWork.Employees.GetEmployeeByEmailAsync(email);
    }

    public async Task AddEmployeeAsync(Employee employee) {
        var validationResult = await _employeeValidator.ValidateAsync(employee);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Employees.AddEmployeeAsync(employee);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee) {
        var validationResult = await _employeeValidator.ValidateAsync(employee);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Employees.UpdateEmployeeAsync(employee);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteEmployeeAsync(Guid id) {
        await _unitOfWork.Employees.DeleteEmployeeAsync(id);
        await _unitOfWork.SaveAsync();
    }
}