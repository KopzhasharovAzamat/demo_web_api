using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Company;

namespace demo_web_api.BLL.Interfaces;

public interface ICompanyService {
    Task<List<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task<Company> AddCompanyAsync(AddCompanyVm addCompanyVm);
    Task<Company> UpdateCompanyAsync(Guid id, UpdateCompanyVm updateVm);
    Task DeleteCompanyAsync(Guid id);
}