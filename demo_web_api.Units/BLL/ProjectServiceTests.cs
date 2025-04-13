using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.Project;
using FluentValidation;
using Moq;
using Xunit;

namespace demo_web_api.Units.BLL;

public class ProjectServiceTests {
    private readonly Mock<IUnitOfWork>                 _unitOfWorkMock;
    private readonly Mock<IValidator<AddProjectVm>>    _addValidatorMock;
    private readonly Mock<IValidator<UpdateProjectVm>> _updateValidatorMock;
    private readonly Mock<IMapper>                     _mapperMock;
    private readonly ProjectService                    _projectService;

    public ProjectServiceTests() {
        _unitOfWorkMock      = new();
        _addValidatorMock    = new();
        _updateValidatorMock = new();
        _mapperMock          = new();

        _addValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddProjectVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectVm>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock.Setup(m => m.Map<Project>(It.IsAny<AddProjectVm>()))
            .Returns((AddProjectVm vm) => new() { Id = Guid.NewGuid(), Name = vm.Name });

        _mapperMock.Setup(m => m.Map(It.IsAny<UpdateProjectVm>(), It.IsAny<Project>()))
            .Callback<UpdateProjectVm, Project>((src, dest) => dest.Name = src.Name);

        _projectService = new(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _addValidatorMock.Object,
            _updateValidatorMock.Object
        );
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
        var addVm = new AddProjectVm { Name = "New Project" };

        _unitOfWorkMock.Setup(u => u.Projects.AddProjectAsync(It.IsAny<Project>()));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _projectService.AddProjectAsync(addVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.Projects.AddProjectAsync(It.IsAny<Project>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        Assert.Equal("New Project", result.Name);
    }

    [Fact]
    public async Task Should_UpdateProject() {
        // Arrange
        var id       = Guid.NewGuid();
        var updateVm = new UpdateProjectVm { Name = "Updated" };
        var existing = new Project { Id           = id, Name = "Old" };

        _unitOfWorkMock.Setup(u => u.Projects.GetProjectByIdAsync(id)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.Projects.UpdateProjectAsync(existing));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        var result = await _projectService.UpdateProjectAsync(id, updateVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.Projects.UpdateProjectAsync(existing), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        Assert.Equal("Updated", result.Name);
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

    [Fact]
    public async Task Should_DeleteProject() {
        // Arrange
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Projects.DeleteProjectAsync(id));
        _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

        // Act
        await _projectService.DeleteProjectAsync(id);

        // Assert
        _unitOfWorkMock.Verify(u => u.Projects.DeleteProjectAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}