using AutoMapper;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.ProjectEmployee;
using FluentValidation;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class ProjectEmployeeServiceTests {
    private readonly Mock<IUnitOfWork>                    _unitOfWorkMock;
    private readonly Mock<IProjectEmployeeRepository>     _projectEmployeeRepoMock;
    private readonly Mock<IValidator<ProjectEmployeeDto>> _validatorMock;
    private readonly Mock<IMapper>                        _mapperMock;

    private readonly ProjectEmployeeService _service;

    public ProjectEmployeeServiceTests() {
        _unitOfWorkMock          = new();
        _projectEmployeeRepoMock = new();
        _validatorMock           = new();
        _mapperMock              = new();

        _unitOfWorkMock.Setup(u => u.ProjectEmployees).Returns(_projectEmployeeRepoMock.Object);

        _service = new(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Should_AssignEmployeesToProject() {
        // Arrange
        var projectId   = Guid.NewGuid();
        var employeeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var dto = new AssignEmployeesDto {
            ProjectId   = projectId,
            EmployeeIds = employeeIds
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ProjectEmployeeDto>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock
            .Setup(m => m.Map<ProjectEmployee>(It.IsAny<ProjectEmployeeDto>()))
            .Returns<ProjectEmployeeDto>(
                d => new() {
                    ProjectId  = d.ProjectId,
                    EmployeeId = d.EmployeeId
                }
            );

        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.AssignEmployeesToProjectAsync(dto);

        // Assert
        foreach (var employeeId in employeeIds) {
            _projectEmployeeRepoMock.Verify(
                repo =>
                    repo.AddProjectEmployeeAsync(
                        It.Is<ProjectEmployee>(
                            pe =>
                                pe.ProjectId == projectId && pe.EmployeeId == employeeId
                        )
                    ),
                Times.Once
            );
        }

        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_RemoveEmployeeFromProject() {
        // Arrange
        var dto = new ProjectEmployeeDto {
            ProjectId  = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid()
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.Is<ProjectEmployeeDto>(x => x.ValidateForDelete), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.RemoveEmployeeFromProjectAsync(dto);

        // Assert
        _projectEmployeeRepoMock.Verify(
            repo =>
                repo.RemoveProjectEmployeeAsync(dto.ProjectId, dto.EmployeeId),
            Times.Once
        );

        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_GetEmployeesByProject() {
        // Arrange
        var projectId = Guid.NewGuid();
        var employees = new List<Employee> {
            new() { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith", MiddleName = "M", Email = "a@x.com" },
            new() { Id = Guid.NewGuid(), FirstName = "Bob", LastName   = "Brown", MiddleName = "N", Email = "b@x.com" }
        };

        _projectEmployeeRepoMock
            .Setup(r => r.GetEmployeesByProjectAsync(projectId))
            .ReturnsAsync(employees);
        
        // Act
        var result = await _service.GetEmployeesByProjectAsync(projectId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Alice", result[0].FullName.Split(" ")[1]);
    }

    [Fact]
    public async Task Should_GetProjectsByEmployee() {
        // Arrange
        var employeeId = Guid.NewGuid();
        var projects = new List<Project> {
            new() { Id = Guid.NewGuid(), Name = "Test 1", StartDate = DateTime.Now },
            new() { Id = Guid.NewGuid(), Name = "Test 2", StartDate = DateTime.Now }
        };

        _projectEmployeeRepoMock
            .Setup(r => r.GetProjectsByEmployeeAsync(employeeId))
            .ReturnsAsync(projects);

        // Act
        var result = await _service.GetProjectsByEmployeeAsync(employeeId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Test 1", result[0].ProjectName);
    }

    [Fact]
    public async Task Should_GetAllProjectEmployees() {
        // Arrange
        var data = new List<ProjectEmployee> {
            new() {
                Project  = new() { Name      = "Project X", StartDate = DateTime.Now },
                Employee = new() { FirstName = "X", LastName          = "Y", MiddleName = "Z", Email = "x@y.com" }
            }
        };

        _projectEmployeeRepoMock.Setup(r => r.GetAllProjectEmployeesAsync()).ReturnsAsync(data);
        _mapperMock.Setup(m => m.Map<List<ProjectEmployeeVm>>(data)).Returns(
            new List<ProjectEmployeeVm> {
                new() {
                    FullName = "Y X Z", Email = "x@y.com", ProjectName = "Project X"
                }
            }
        );

        // Act
        var result = await _service.GetAllProjectEmployeesAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Y X Z", result[0].FullName);
    }
}