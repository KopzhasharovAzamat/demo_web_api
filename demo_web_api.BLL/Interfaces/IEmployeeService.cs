using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Employee;

namespace demo_web_api.BLL.Interfaces;

public interface IEmployeeService {
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<Employee> AddEmployeeAsync(AddEmployeeVm addEmployeeVm);
    Task<Employee> UpdateEmployeeAsync(Guid id, UpdateEmployeeVm updateEmployeeVm);
    Task DeleteEmployeeAsync(Guid id);
}