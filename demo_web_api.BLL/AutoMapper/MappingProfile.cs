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
        CreateMap<Project, ProjectVm>()
            .ForMember(
                dest => dest.ProjectManagerName,
                opt => opt.MapFrom(
                    src => src.ProjectManager != null ?
                        $"{src.ProjectManager.LastName} {src.ProjectManager.FirstName}" :
                        null
                )
            )
            .ForMember(
                dest => dest.CustomerCompanyName,
                opt => opt.MapFrom(src => src.CustomerCompany.Name)
            )
            .ForMember(
                dest => dest.ContractorCompanyName,
                opt => opt.MapFrom(src => src.ContractorCompany.Name)
            );

        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>();

        // Map for Company
        CreateMap<Company, CompanyDto>();
        CreateMap<Company, CompanyVm>();
        CreateMap<CompanyDto, Company>();

        CreateMap<Company, AddCompanyVm>();
        CreateMap<AddCompanyVm, Company>();

        CreateMap<Company, UpdateCompanyVm>();
        CreateMap<UpdateCompanyVm, Company>();

        // Map for Employee
        CreateMap<Employee, EmployeeVm>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeDto, Employee>();

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