using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class ProjectServiceTests {
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProjectService    _projectService;

    public ProjectServiceTests() {
        _unitOfWorkMock = new();
        _projectService = new(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_GetAllProjects() {
        // Arrange
        var expectedProjects = new List<Project> {
            new() { Id = Guid.NewGuid(), Name = "Test 1" },
            new() { Id = Guid.NewGuid(), Name = "Test 2" }
        };

        _unitOfWorkMock
            .Setup(u => u.Projects.GetAllProjectsAsync())
            .ReturnsAsync(expectedProjects);

        // Act
        var result = await _projectService.GetAllProjectsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_AddProject() {
        // Arrange
        var newProject = new Project {
            Id   = Guid.NewGuid(),
            Name = "New Project"
        };

        _unitOfWorkMock.Setup(u => u.Projects.AddProjectAsync(newProject));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _projectService.AddProjectAsync(newProject);

        // Assert
        _unitOfWorkMock.Verify(u => u.Projects.AddProjectAsync(newProject), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_GetProjectById() {
        // Arrange
        var id              = Guid.NewGuid();
        var expectedProject = new Project { Id = id, Name = "Project X" };

        _unitOfWorkMock
            .Setup(u => u.Projects.GetProjectByIdAsync(id))
            .ReturnsAsync(expectedProject);

        // Act
        var result = await _projectService.GetProjectByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Project X", result.Name);
    }
}