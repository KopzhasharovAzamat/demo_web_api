using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class EmployeeServiceTests {
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EmployeeService   _employeeService;

    public EmployeeServiceTests() {
        _unitOfWorkMock  = new();
        _employeeService = new(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_GetAllEmployees() {
        var expectedEmployees = new List<Employee> {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _unitOfWorkMock
            .Setup(u => u.Employees.GetAllEmployeesAsync())
            .ReturnsAsync(expectedEmployees);

        var result = await _employeeService.GetAllEmployeesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_GetEmployeeById() {
        var id       = Guid.NewGuid();
        var expected = new Employee { Id = id, FirstName = "Alex" };

        _unitOfWorkMock
            .Setup(u => u.Employees.GetEmployeeByIdAsync(id))
            .ReturnsAsync(expected);

        var result = await _employeeService.GetEmployeeByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal("Alex", result.FirstName);
    }

    [Fact]
    public async Task Should_AddEmployee() {
        var newEmployee = new Employee {
            Id        = Guid.NewGuid(),
            FirstName = "Alice"
        };

        _unitOfWorkMock.Setup(u => u.Employees.AddEmployeeAsync(newEmployee));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _employeeService.AddEmployeeAsync(newEmployee);

        _unitOfWorkMock.Verify(u => u.Employees.AddEmployeeAsync(newEmployee), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_UpdateEmployee() {
        var employee = new Employee { Id = Guid.NewGuid() };

        _unitOfWorkMock.Setup(u => u.Employees.UpdateEmployeeAsync(employee));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _employeeService.UpdateEmployeeAsync(employee);

        _unitOfWorkMock.Verify(u => u.Employees.UpdateEmployeeAsync(employee), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_DeleteEmployee() {
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Employees.DeleteEmployeeAsync(id));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _employeeService.DeleteEmployeeAsync(id);

        _unitOfWorkMock.Verify(u => u.Employees.DeleteEmployeeAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}