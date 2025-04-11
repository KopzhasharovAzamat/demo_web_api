using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository {
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync() {
        return await _dbContext.Employees.ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id) {
        return await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email) {
        return await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmployeeExistsAsync(Guid id) {
        return await _dbContext.Employees.AnyAsync(x => x.Id == id);
    }

    public void AddEmployeeAsync(Employee employee) {
        _dbContext.Employees.Add(employee);
    }

    public void UpdateEmployeeAsync(Employee employee) {
        _dbContext.Employees.Update(employee);
    }

    public async Task DeleteEmployeeAsync(Guid id) {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee != null) {
            _dbContext.Employees.Remove(employee);
        }
    }
}