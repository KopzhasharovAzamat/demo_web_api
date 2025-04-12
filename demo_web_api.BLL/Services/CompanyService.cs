using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class CompanyService : ICompanyService {
    private readonly IUnitOfWork         _unitOfWork;
    private readonly IValidator<Company> _companyValidator;

    public CompanyService(IUnitOfWork unitOfWork, IValidator<Company> companyValidator) {
        _unitOfWork       = unitOfWork;
        _companyValidator = companyValidator;
    }

    public async Task<List<Company>> GetAllCompaniesAsync() {
        return await _unitOfWork.Companies.GetAllCompaniesAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid id) {
        return await _unitOfWork.Companies.GetCompanyByIdAsync(id);
    }

    public async Task AddCompanyAsync(Company company) {
        var validationResult = await _companyValidator.ValidateAsync(company);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Companies.AddCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Company company) {
        var validationResult = await _companyValidator.ValidateAsync(company);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Companies.UpdateCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCompanyAsync(Guid id) {
        await _unitOfWork.Companies.DeleteCompanyAsync(id);
        await _unitOfWork.SaveAsync();
    }
}