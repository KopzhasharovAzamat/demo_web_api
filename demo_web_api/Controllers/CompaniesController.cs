using demo_web_api.BLL.Interfaces;
using demo_web_api.DTOs.Company;
using Microsoft.AspNetCore.Mvc;

namespace demo_web_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase {
    private readonly ICompanyService _companyService;

    public CompaniesController(
        ICompanyService companyService
    ) {
        _companyService = companyService;
    }

    // Get all companies
    [HttpGet]
    public async Task<IActionResult> GetAllCompanies() {
        var companies = await _companyService.GetAllCompaniesAsync();

        return Ok(companies);
    }

    // Get company by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id) {
        var foundCompany = await _companyService.GetCompanyByIdAsync(id);

        if (foundCompany is null) return NotFound();

        return Ok(foundCompany);
    }

    // Create company
    [HttpPost]
    public async Task<IActionResult> CreateCompany(AddCompanyVm vm) {
        var created = await _companyService.AddCompanyAsync(vm);

        return CreatedAtAction(nameof(GetCompanyById), new { id = created.Id }, created);
    }

    // Update company
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, UpdateCompanyVm vm) {
        var updated = await _companyService.UpdateCompanyAsync(id, vm);

        return Ok(updated);
    }

    // Delete company
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id) {
        await _companyService.DeleteCompanyAsync(id);

        return NoContent();
    }
}