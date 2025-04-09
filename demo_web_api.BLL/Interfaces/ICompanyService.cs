using demo_web_api.DAL.Entities;

namespace demo_web_api.BLL.Interfaces;

public interface ICompanyService {
    Task<List<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task AddCompanyAsync(Company company);
    Task UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(Guid id);
}