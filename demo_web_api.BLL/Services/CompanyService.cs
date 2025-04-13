using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Company;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class CompanyService : ICompanyService {
    private readonly IUnitOfWork                 _unitOfWork;
    private readonly IValidator<AddCompanyVm>    _addValidator;
    private readonly IValidator<UpdateCompanyVm> _updateValidator;
    private readonly IMapper                     _mapper;

    public CompanyService(
        IUnitOfWork                 unitOfWork,
        IValidator<AddCompanyVm>    addValidator,
        IValidator<UpdateCompanyVm> updateValidator,
        IMapper                     mapper
    ) {
        _unitOfWork      = unitOfWork;
        _addValidator    = addValidator;
        _updateValidator = updateValidator;
        _mapper          = mapper;
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
    public async Task<Company> AddCompanyAsync(AddCompanyVm addCompanyVm) {
        var validationResult = await _addValidator.ValidateAsync(addCompanyVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var newCompany = _mapper.Map<Company>(addCompanyVm);
        newCompany.Id = Guid.NewGuid();

        _unitOfWork.Companies.AddCompanyAsync(newCompany);
        await _unitOfWork.SaveAsync();

        return newCompany;
    }

    // Update company
    public async Task<Company> UpdateCompanyAsync(Guid id, UpdateCompanyVm updateVm) {
        var validationResult = await _updateValidator.ValidateAsync(updateVm);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        var existingCompany = await _unitOfWork.Companies.GetCompanyByIdAsync(id);
        if (existingCompany is null) {
            throw new("Company not found");
        }

        _mapper.Map(updateVm, existingCompany);
        _unitOfWork.Companies.UpdateCompanyAsync(existingCompany);
        await _unitOfWork.SaveAsync();

        return existingCompany;
    }

    // Delete company
    public async Task DeleteCompanyAsync(Guid id) {
        await _unitOfWork.Companies.DeleteCompanyAsync(id);
        await _unitOfWork.SaveAsync();
    }
}