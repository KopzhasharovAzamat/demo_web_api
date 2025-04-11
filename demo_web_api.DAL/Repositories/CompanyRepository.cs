using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories;

public class CompanyRepository : ICompanyRepository {
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    public async Task<List<Company>> GetAllCompaniesAsync() {
        return await _dbContext.Companies.ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid id) {
        return await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void AddCompanyAsync(Company company) {
        _dbContext.Companies.Add(company);
    }

    public void UpdateCompanyAsync(Company company) {
        _dbContext.Companies.Update(company);
    }

    public async Task DeleteCompanyAsync(Guid id) {
        var company = await _dbContext.Companies.FindAsync(id);
        if (company != null) {
            _dbContext.Companies.Remove(company);
        }
    }
}