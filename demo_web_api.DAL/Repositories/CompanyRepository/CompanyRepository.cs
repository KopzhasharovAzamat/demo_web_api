using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.Repositories.CompanyRepository;

public class CompanyRepository : ICompanyRepository {
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext context) {
        _dbContext = context;
    }

    public async Task<List<Company>> GetAllCompaniesAsync() {
        return await _dbContext.Companies.ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(int id) {
        return await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddCompanyAsync(Company company) {
        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCompanyAsync(Company company) {
        _dbContext.Companies.Update(company);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(int id) {
        var company = await _dbContext.Companies.FindAsync(id);
        if (company != null) {
            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }
    }
}