using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

public class CompaniesController : ControllerBase {
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService) {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies() {
        var companies = await _companyService.GetAllCompaniesAsync();
        var result = companies.Select(
            x => new CompanyDto {
                Name = x.Name
            }
        );

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id) {
        var existingCompany = await _companyService.GetCompanyByIdAsync(id);

        if (existingCompany is null) return NotFound();

        var foundCompany = new CompanyDto() {
            Id   = existingCompany.Id,
            Name = existingCompany.Name
        };

        return Ok(foundCompany);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(CompanyDto companyDto) {
        var newCompany = new Company {
            Id   = Guid.NewGuid(),
            Name = companyDto.Name
        };

        await _companyService.AddCompanyAsync(newCompany);

        return CreatedAtAction(nameof(GetCompanyById), new { id = newCompany.Id }, companyDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyDto companyDto) {
        var existingCompany = await _companyService.GetCompanyByIdAsync(id);
        if (existingCompany is null) {
            return NotFound();
        }

        existingCompany.Name = companyDto.Name;
        await _companyService.UpdateCompanyAsync(existingCompany);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id) {
        await _companyService.DeleteCompanyAsync(id);

        return NoContent();
    }
}