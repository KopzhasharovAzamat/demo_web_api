using demo_web_api.DAL.Repositories.CompanyRepository;
using demo_web_api.DAL.Repositories.EmployeeRepository;
using demo_web_api.DAL.Repositories.ProjectRepository;

namespace demo_web_api.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable {
    IProjectRepository  Projects  { get; }
    IEmployeeRepository Employees { get; }
    ICompanyRepository  Companies { get; }
    Task<int> SaveAsync();
}