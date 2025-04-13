using AutoMapper;
using demo_web_api.DAL.Entities;
using demo_web_api.DTOs.Company;
using demo_web_api.DTOs.Employee;
using demo_web_api.DTOs.Project;
using demo_web_api.DTOs.ProjectEmployee;

namespace demo_web_api.BLL.AutoMapper;

public class MappingProfile : Profile {
    public MappingProfile() {
        // Map for Project
        CreateMap<Project, AddProjectVm>();
        CreateMap<AddProjectVm, Project>();

        CreateMap<Project, UpdateProjectVm>();
        CreateMap<UpdateProjectVm, Project>();

        // Map for Company
        CreateMap<Company, AddCompanyVm>();
        CreateMap<AddCompanyVm, Company>();

        CreateMap<Company, UpdateCompanyVm>();
        CreateMap<UpdateCompanyVm, Company>();

        // Map for Employee
        CreateMap<Employee, AddEmployeeVm>();
        CreateMap<AddEmployeeVm, Employee>();

        CreateMap<Employee, UpdateEmployeeVm>();
        CreateMap<UpdateEmployeeVm, Employee>();

        // Map for ProjectEmployee
        CreateMap<ProjectEmployee, ProjectEmployeeVm>()
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.Employee.LastName} {src.Employee.FirstName} {src.Employee.MiddleName}")
            )
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Employee.Email))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Project.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Project.EndDate));
    }
}