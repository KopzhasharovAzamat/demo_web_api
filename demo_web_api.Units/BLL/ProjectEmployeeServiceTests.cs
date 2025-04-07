using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class ProjectEmployeeServiceTests {
    private readonly Mock<IUnitOfWork>                _unitOfWorkMock;
    private readonly Mock<IProjectEmployeeRepository> _projectEmployeeRepoMock;
    private readonly ProjectEmployeeService           _service;

    public ProjectEmployeeServiceTests() {
        _unitOfWorkMock          = new();
        _projectEmployeeRepoMock = new();
        _unitOfWorkMock.Setup(u => u.ProjectEmployees).Returns(_projectEmployeeRepoMock.Object);

        _service = new(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_AssignEmployeeToProject_When_ProjectEmployeeDoesNotExist() {
        // Arrange
        var projectId  = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        _projectEmployeeRepoMock
            .Setup(repo => repo.ExistsProjectEmployeeAsync(projectId, employeeId))
            .ReturnsAsync(false);

        _projectEmployeeRepoMock
            .Setup(repo => repo.AddProjectEmployeeAsync(It.IsAny<ProjectEmployee>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.AssignEmployeeToProjectAsync(projectId, employeeId);

        // Assert
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

        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Not_AssignEmployeeToProject_When_ProjectEmployeeAlreadyExists() {
        // Arrange
        var projectId  = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        _projectEmployeeRepoMock
            .Setup(repo => repo.ExistsProjectEmployeeAsync(projectId, employeeId))
            .ReturnsAsync(true);

        // Act
        await _service.AssignEmployeeToProjectAsync(projectId, employeeId);

        // Assert
        _projectEmployeeRepoMock.Verify(
            repo =>
                repo.AddProjectEmployeeAsync(It.IsAny<ProjectEmployee>()),
            Times.Never
        );

        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_RemoveEmployeeFromProject() {
        var projectId  = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        _projectEmployeeRepoMock
            .Setup(repo => repo.RemoveProjectEmployeeAsync(projectId, employeeId))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        await _service.RemoveEmployeeFromProjectAsync(projectId, employeeId);

        _projectEmployeeRepoMock.Verify(repo => repo.RemoveProjectEmployeeAsync(projectId, employeeId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_GetEmployeesByProject() {
        var projectId = Guid.NewGuid();

        var expected = new List<Employee> {
            new() { Id = Guid.NewGuid(), FirstName = "Anna" },
            new() { Id = Guid.NewGuid(), FirstName = "Bob" }
        };

        _projectEmployeeRepoMock
            .Setup(repo => repo.GetEmployeesByProjectAsync(projectId))
            .ReturnsAsync(expected);

        var result = await _service.GetEmployeesByProjectAsync(projectId);

        Assert.Equal(2, result.Count);
        Assert.Equal("Anna", result[0].FirstName);
    }

    [Fact]
    public async Task Should_GetProjectsByEmployee() {
        var employeeId = Guid.NewGuid();

        var expected = new List<Project> {
            new() { Id = Guid.NewGuid(), Name = "Proj1" },
            new() { Id = Guid.NewGuid(), Name = "Proj2" }
        };

        _projectEmployeeRepoMock
            .Setup(repo => repo.GetProjectsByEmployeeAsync(employeeId))
            .ReturnsAsync(expected);

        var result = await _service.GetProjectsByEmployeeAsync(employeeId);

        Assert.Equal(2, result.Count);
        Assert.Equal("Proj1", result[0].Name);
    }
}