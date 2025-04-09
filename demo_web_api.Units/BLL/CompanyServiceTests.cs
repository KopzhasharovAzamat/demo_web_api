using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class CompanyServiceTests {
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CompanyService    _companyService;

    public CompanyServiceTests() {
        _unitOfWorkMock = new();
        _companyService = new(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_GetAllCompanies() {
        var expected = new List<Company> {
            new() { Id = Guid.NewGuid(), Name = "Google" },
            new() { Id = Guid.NewGuid(), Name = "Microsoft" }
        };

        _unitOfWorkMock.Setup(u => u.Companies.GetAllCompaniesAsync()).ReturnsAsync(expected);

        var result = await _companyService.GetAllCompaniesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_GetCompanyById() {
        var id       = Guid.NewGuid();
        var expected = new Company { Id = id, Name = "Apple" };

        _unitOfWorkMock.Setup(u => u.Companies.GetCompanyByIdAsync(id)).ReturnsAsync(expected);

        var result = await _companyService.GetCompanyByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal("Apple", result.Name);
    }

    [Fact]
    public async Task Should_AddCompany() {
        var company = new Company { Id = Guid.NewGuid(), Name = "Amazon" };

        _unitOfWorkMock.Setup(u => u.Companies.AddCompanyAsync(company));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _companyService.AddCompanyAsync(company);

        _unitOfWorkMock.Verify(u => u.Companies.AddCompanyAsync(company), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_UpdateCompany() {
        var company = new Company { Id = Guid.NewGuid(), Name = "Netflix" };

        _unitOfWorkMock.Setup(u => u.Companies.UpdateCompanyAsync(company));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _companyService.UpdateCompanyAsync(company);

        _unitOfWorkMock.Verify(u => u.Companies.UpdateCompanyAsync(company), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_DeleteCompany() {
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Companies.DeleteCompanyAsync(id));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _companyService.DeleteCompanyAsync(id);

        _unitOfWorkMock.Verify(u => u.Companies.DeleteCompanyAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}