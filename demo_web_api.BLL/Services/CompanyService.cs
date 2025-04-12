using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Company;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class CompanyService : ICompanyService {
    private readonly IUnitOfWork         _unitOfWork;
    private readonly IValidator<Company> _validator;
    private readonly IMapper             _mapper;

    public CompanyService(
        IUnitOfWork         unitOfWork,
        IValidator<Company> validator,
        IMapper             mapper
    ) {
        _unitOfWork = unitOfWork;
        _validator  = validator;
        _mapper     = mapper;
    }

    // Get all companies
    public async Task<List<Company>> GetAllCompaniesAsync() {
        return await _unitOfWork.Companies.GetAllCompaniesAsync();
    }

    // Get company by id
    public async Task<Company?> GetCompanyByIdAsync(Guid id) {
        return await _unitOfWork.Companies.GetCompanyByIdAsync(id);
    }

    // Add company
    public async Task AddCompanyAsync(AddCompanyVm addCompanyVm) {
        var validationResult = await _validator.ValidateAsync(addCompanyVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var newCompany = _mapper.Map<Company>(companyDto);
        newCompany.Id = Guid.NewGuid();

        _unitOfWork.Companies.AddCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    // Update company
    public async Task UpdateCompanyAsync(Company company) {
        var validationResult = await _validator.ValidateAsync(company);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.Companies.UpdateCompanyAsync(company);
        await _unitOfWork.SaveAsync();
    }

    // Delete company
    public async Task DeleteCompanyAsync(Guid id) {
        await _unitOfWork.Companies.DeleteCompanyAsync(id);
        await _unitOfWork.SaveAsync();
    }
}