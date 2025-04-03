using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Repositories.EmployeeRepository;

public interface IEmployeeRepository {
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(Guid id);
}