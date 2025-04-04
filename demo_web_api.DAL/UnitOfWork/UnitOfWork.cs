using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Repositories.CompanyRepository;
using demo_web_api.DAL.Repositories.EmployeeRepository;
using demo_web_api.DAL.Repositories.ProjectRepository;

namespace demo_web_api.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork {
    private readonly ApplicationDbContext _dbContext;
    public           IProjectRepository   Projects  { get; }
    public           IEmployeeRepository  Employees { get; }
    public           ICompanyRepository   Companies { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IProjectRepository   projectRepository,
        IEmployeeRepository  employeeRepository,
        ICompanyRepository   companyRepository
    ) {
        _dbContext = context;
        Projects   = projectRepository;
        Employees  = employeeRepository;
        Companies  = companyRepository;
    }

    public async Task<int> SaveAsync() {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose() {
        _dbContext.Dispose();
    }
}