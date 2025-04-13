using AutoMapper;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Employee;
using FluentValidation;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class EmployeeServiceTests {
    private readonly Mock<IUnitOfWork>                  _unitOfWorkMock;
    private readonly Mock<IValidator<AddEmployeeVm>>    _addValidatorMock;
    private readonly Mock<IValidator<UpdateEmployeeVm>> _updateValidatorMock;
    private readonly Mock<IMapper>                      _mapperMock;
    private readonly EmployeeService                    _employeeService;

    public EmployeeServiceTests() {
        _unitOfWorkMock      = new();
        _addValidatorMock    = new();
        _updateValidatorMock = new();
        _mapperMock          = new();

        _addValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddEmployeeVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateEmployeeVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<AddEmployeeVm>()))
            .Returns(
                (AddEmployeeVm vm) => new() {
                    Id         = Guid.NewGuid(),
                    FirstName  = vm.FirstName,
                    LastName   = vm.LastName,
                    MiddleName = vm.MiddleName,
                    Email      = vm.Email
                }
            );

        _mapperMock.Setup(m => m.Map(It.IsAny<UpdateEmployeeVm>(), It.IsAny<Employee>()))
            .Callback<UpdateEmployeeVm, Employee>(
                (src, dest) => {
                    dest.FirstName  = src.FirstName;
                    dest.LastName   = src.LastName;
                    dest.MiddleName = src.MiddleName;
                    dest.Email      = src.Email;
                }
            );

        _employeeService = new EmployeeService(
            _unitOfWorkMock.Object,
            _addValidatorMock.Object,
            _updateValidatorMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Should_GetAllEmployees() {
        // Arrange
        var expectedEmployees = new List<Employee> {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _unitOfWorkMock
            .Setup(u => u.Employees.GetAllEmployeesAsync())
            .ReturnsAsync(expectedEmployees);

        // Act
        var result = await _employeeService.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_GetEmployeeById() {
        // Arrange
        var id       = Guid.NewGuid();
        var expected = new Employee { Id = id, FirstName = "Alex" };

        _unitOfWorkMock
            .Setup(u => u.Employees.GetEmployeeByIdAsync(id))
            .ReturnsAsync(expected);

        // Act
        var result = await _employeeService.GetEmployeeByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alex", result.FirstName);
    }

    [Fact]
    public async Task Should_AddEmployee() {
        // Arrange
        var addVm = new AddEmployeeVm {
            FirstName  = "Emily",
            LastName   = "Johnson",
            MiddleName = "A.",
            Email      = "emily@example.com"
        };

        _unitOfWorkMock.Setup(u => u.Employees.AddEmployeeAsync(It.IsAny<Employee>()));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _employeeService.AddEmployeeAsync(addVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.Employees.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        Assert.Equal("Emily", result.FirstName);
    }

    [Fact]
    public async Task Should_UpdateEmployee() {
        // Arrange
        var id = Guid.NewGuid();
        var updateVm = new UpdateEmployeeVm {
            FirstName  = "Updated",
            LastName   = "User",
            MiddleName = "M.",
            Email      = "updated@example.com"
        };

        var existing = new Employee {
            Id        = id,
            FirstName = "Old",
            LastName  = "User",
            Email     = "old@example.com"
        };

        _unitOfWorkMock.Setup(u => u.Employees.GetEmployeeByIdAsync(id)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.Employees.UpdateEmployeeAsync(existing));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _employeeService.UpdateEmployeeAsync(id, updateVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.Employees.UpdateEmployeeAsync(existing), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        Assert.Equal("Updated", result.FirstName);
    }

    [Fact]
    public async Task Should_DeleteEmployee() {
        // Arrange
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Employees.DeleteEmployeeAsync(id));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _employeeService.DeleteEmployeeAsync(id);

        // Assert
        _unitOfWorkMock.Verify(u => u.Employees.DeleteEmployeeAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}