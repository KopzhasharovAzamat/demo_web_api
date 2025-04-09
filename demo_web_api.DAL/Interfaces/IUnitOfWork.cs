namespace demo_web_api.DAL.Interfaces;

public interface IUnitOfWork : IDisposable {
    IProjectRepository         Projects         { get; }
    IEmployeeRepository        Employees        { get; }
    ICompanyRepository         Companies        { get; }
    IProjectEmployeeRepository ProjectEmployees { get; }
    Task<int> SaveAsync();
}