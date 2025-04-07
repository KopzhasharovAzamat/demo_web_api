using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;

namespace demo_web_api.DAL.Repositories;

public class UnitOfWork : IUnitOfWork {
    private readonly ApplicationDbContext       _dbContext;
    public           IProjectRepository         Projects         { get; }
    public           IEmployeeRepository        Employees        { get; }
    public           ICompanyRepository         Companies        { get; }
    public           IProjectEmployeeRepository ProjectEmployees { get; }

    public UnitOfWork(
        ApplicationDbContext       context,
        IProjectRepository         projectRepository,
        IEmployeeRepository        employeeRepository,
        ICompanyRepository         companyRepository,
        IProjectEmployeeRepository projectEmployeeRepository
    ) {
        _dbContext       = context;
        Projects         = projectRepository;
        Employees        = employeeRepository;
        Companies        = companyRepository;
        ProjectEmployees = projectEmployeeRepository;
    }

    public async Task<int> SaveAsync() {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose() {
        _dbContext.Dispose();
    }
}