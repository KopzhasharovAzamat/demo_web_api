using AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DTOs.ProjectEmployee;
using FluentValidation;

namespace demo_web_api.BLL.Services;

public class ProjectEmployeeService : IProjectEmployeeService {
    private readonly IUnitOfWork                    _unitOfWork;
    private readonly IMapper                        _mapper;
    private readonly IValidator<ProjectEmployeeDto> _validator;

    public ProjectEmployeeService(
        IUnitOfWork                    unitOfWork,
        IMapper                        mapper,
        IValidator<ProjectEmployeeDto> validator
    ) {
        _unitOfWork = unitOfWork;
        _mapper     = mapper;
        _validator  = validator;
    }

    public async Task<List<ProjectEmployeeVm>> GetAllProjectEmployeesAsync() {
        var result = await _unitOfWork.ProjectEmployees.GetAllProjectEmployeesAsync();

        return _mapper.Map<List<ProjectEmployeeVm>>(result);
    }

    public async Task<List<ProjectEmployeeVm>> GetEmployeesByProjectAsync(Guid projectId) {
        var employees = await _unitOfWork.ProjectEmployees.GetEmployeesByProjectAsync(projectId);

        return employees
            .Select(
                e => new ProjectEmployeeVm {
                    EmployeeId = e.Id,
                    FullName   = $"{e.LastName} {e.FirstName} {e.MiddleName}",
                    Email      = e.Email,
                    ProjectId  = projectId
                }
            ).ToList();
    }

    public async Task<List<ProjectEmployeeVm>> GetProjectsByEmployeeAsync(Guid employeeId) {
        var projects = await _unitOfWork.ProjectEmployees.GetProjectsByEmployeeAsync(employeeId);

        return projects
            .Select(
                p => new ProjectEmployeeVm {
                    ProjectId   = p.Id,
                    ProjectName = p.Name,
                    StartDate   = p.StartDate,
                    EndDate     = p.EndDate ?? null,
                    EmployeeId  = employeeId
                }
            ).ToList();
    }

    public async Task AssignEmployeesToProjectAsync(AssignEmployeesDto dto) {
        foreach (var employeeId in dto.EmployeeIds) {
            var singleDto = new ProjectEmployeeDto {
                ProjectId  = dto.ProjectId,
                EmployeeId = employeeId
            };

            var validationResult = await _validator.ValidateAsync(singleDto);
            if (!validationResult.IsValid) {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<ProjectEmployee>(singleDto);
            _unitOfWork.ProjectEmployees.AddProjectEmployeeAsync(entity);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task RemoveEmployeeFromProjectAsync(ProjectEmployeeDto dto) {
        dto.ValidateForDelete = true;

        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid) {
            throw new ValidationException(validationResult.Errors);
        }

        await _unitOfWork.ProjectEmployees.RemoveProjectEmployeeAsync(dto.ProjectId, dto.EmployeeId);
        await _unitOfWork.SaveAsync();
    }
}