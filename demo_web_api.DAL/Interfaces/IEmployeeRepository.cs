using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Interfaces;

public interface IEmployeeRepository {
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<bool> EmployeeExistsAsync(Guid id);
    void AddEmployeeAsync(Employee employee);
    void UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(Guid id);
}