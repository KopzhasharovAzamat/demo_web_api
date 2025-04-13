using AutoMapper;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Company;
using FluentValidation;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class CompanyServiceTests {
    private readonly Mock<IUnitOfWork>                 _unitOfWorkMock;
    private readonly Mock<IValidator<AddCompanyVm>>    _addValidatorMock;
    private readonly Mock<IValidator<UpdateCompanyVm>> _updateValidatorMock;
    private readonly Mock<IMapper>                     _mapperMock;
    private readonly CompanyService                    _companyService;

    public CompanyServiceTests() {
        _unitOfWorkMock      = new();
        _addValidatorMock    = new();
        _updateValidatorMock = new();
        _mapperMock          = new();

        _addValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddCompanyVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateCompanyVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock.Setup(m => m.Map<Company>(It.IsAny<AddCompanyVm>()))
            .Returns((AddCompanyVm src) => new() { Id = Guid.NewGuid(), Name = src.Name });

        _mapperMock.Setup(m => m.Map(It.IsAny<UpdateCompanyVm>(), It.IsAny<Company>()))
            .Callback<UpdateCompanyVm, Company>((src, dest) => dest.Name = src.Name);

        _companyService = new(
            _unitOfWorkMock.Object,
            _addValidatorMock.Object,
            _updateValidatorMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Should_GetAllCompanies() {
        // Arrange
        var expected = new List<Company> {
            new() { Id = Guid.NewGuid(), Name = "Google" },
            new() { Id = Guid.NewGuid(), Name = "Microsoft" }
        };

        _unitOfWorkMock.Setup(u => u.Companies.GetAllCompaniesAsync()).ReturnsAsync(expected);

        // Act
        var result = await _companyService.GetAllCompaniesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_GetCompanyById() {
        // Arrange
        var id       = Guid.NewGuid();
        var expected = new Company { Id = id, Name = "Apple" };

        _unitOfWorkMock.Setup(u => u.Companies.GetCompanyByIdAsync(id)).ReturnsAsync(expected);

        // Act
        var result = await _companyService.GetCompanyByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Apple", result.Name);
    }

    [Fact]
    public async Task Should_AddCompany() {
        // Arrange
        var addVm = new AddCompanyVm { Name = "Amazon" };

        _unitOfWorkMock.Setup(u => u.Companies.AddCompanyAsync(It.IsAny<Company>()));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _companyService.AddCompanyAsync(addVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.Companies.AddCompanyAsync(It.Is<Company>(c => c.Name == "Amazon")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);

        Assert.Equal("Amazon", result.Name);
    }

    [Fact]
    public async Task Should_UpdateCompany() {
        // Arrange
        var id       = Guid.NewGuid();
        var existing = new Company { Id           = id, Name = "Old" };
        var updateVm = new UpdateCompanyVm { Name = "New" };

        _unitOfWorkMock.Setup(u => u.Companies.GetCompanyByIdAsync(id)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.Companies.UpdateCompanyAsync(existing));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _companyService.UpdateCompanyAsync(id, updateVm);

        // Assert
        Assert.Equal("New", result.Name);
        _unitOfWorkMock.Verify(u => u.Companies.UpdateCompanyAsync(existing), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_DeleteCompany() {
        // Arrange
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Companies.DeleteCompanyAsync(id));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _companyService.DeleteCompanyAsync(id);

        // Assert
        _unitOfWorkMock.Verify(u => u.Companies.DeleteCompanyAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}