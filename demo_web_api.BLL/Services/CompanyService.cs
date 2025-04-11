using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;

namespace demo_web_api.BLL.Services;

public class CompanyService : ICompanyService {
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Company>> GetAllCompaniesAsync() {
        return await _unitOfWork.Companies.GetAllCompaniesAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid id) {
        return await _unitOfWork.Companies.GetCompanyByIdAsync(id);
    }

    public async Task AddCompanyAsync(Company company) {
        _unitOfWork.Companies.AddCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Company company) {
        _unitOfWork.Companies.UpdateCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCompanyAsync(Guid id) {
        await _unitOfWork.Companies.DeleteCompanyAsync(id);
        await _unitOfWork.SaveAsync();
    }
}