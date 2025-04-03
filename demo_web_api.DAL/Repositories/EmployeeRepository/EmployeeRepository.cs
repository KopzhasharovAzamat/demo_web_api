using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories.EmployeeRepository;

public class EmployeeRepository : IEmployeeRepository {
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync() {
        return await _dbContext.Employees.ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id) {
        return await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddEmployeeAsync(Employee employee) {
        _dbContext.Add(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee) {
        _dbContext.Update(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id) {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee != null) {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}