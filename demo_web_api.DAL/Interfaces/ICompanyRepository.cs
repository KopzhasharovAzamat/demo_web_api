using demo_web_api.DAL.Entities;

namespace demo_web_api.DAL.Interfaces;

public interface ICompanyRepository {
    Task<List<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(Guid id);
    void AddCompanyAsync(Company company);
    void UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(Guid id);
}