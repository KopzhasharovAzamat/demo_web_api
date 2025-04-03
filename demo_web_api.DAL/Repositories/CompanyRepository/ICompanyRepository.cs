using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Repositories.CompanyRepository;

public interface ICompanyRepository {
    Task<List<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(int id);
    Task AddCompanyAsync(Company company);
    Task UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(int id);
}