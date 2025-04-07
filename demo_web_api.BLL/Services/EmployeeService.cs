using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DAL.Repositories;

namespace demo_web_api.BLL.Services;

public class EmployeeService : IEmployeeService {
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync() {
        return await _unitOfWork.Employees.GetAllEmployeesAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id) {
        return await _unitOfWork.Employees.GetEmployeeByIdAsync(id);
    }

    public async Task AddEmployeeAsync(Employee employee) {
        await _unitOfWork.Employees.AddEmployeeAsync(employee);
    }

    public async Task UpdateEmployeeAsync(Employee employee) {
        await _unitOfWork.Employees.UpdateEmployeeAsync(employee);
    }

    public async Task DeleteEmployeeAsync(Guid id) {
        await _unitOfWork.Employees.DeleteEmployeeAsync(id);
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email) {
        return await _unitOfWork.Employees.GetEmployeeByEmailAsync(email);
    }
}