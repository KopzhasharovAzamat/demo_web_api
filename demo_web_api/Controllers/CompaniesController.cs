using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Company;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase {
    private readonly ICompanyService _companyService;
    private readonly IMapper         _mapper;

    public CompaniesController(
        ICompanyService companyService,
        IMapper         mapper
    ) {
        _companyService = companyService;
        _mapper         = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies() {
        var companies = await _companyService.GetAllCompaniesAsync();
        var result    = _mapper.Map<List<CompanyVm>>(companies);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id) {
        var existingCompany = await _companyService.GetCompanyByIdAsync(id);

        if (existingCompany is null) return NotFound();

        var result = _mapper.Map<CompanyVm>(existingCompany);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(CompanyDto companyDto) {
        var newCompany = _mapper.Map<Company>(companyDto);
        newCompany.Id = Guid.NewGuid();

        await _companyService.AddCompanyAsync(newCompany);

        return CreatedAtAction(nameof(GetCompanyById), new { id = newCompany.Id }, companyDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyDto companyDto) {
        var existingCompany = await _companyService.GetCompanyByIdAsync(id);
        if (existingCompany is null) {
            return NotFound();
        }

        _mapper.Map(companyDto, existingCompany);

        await _companyService.UpdateCompanyAsync(existingCompany);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id) {
        await _companyService.DeleteCompanyAsync(id);

        return NoContent();
    }
}