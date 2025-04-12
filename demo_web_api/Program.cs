using demo_web_api.BLL.Interfaces;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DAL.Repositories;
using demo_web_api.DTOs.Company;
using demo_web_api.DTOs.Employee;
using demo_web_api.DTOs.Project;
using demo_web_api.DTOs.ProjectEmployee;
using demo_web_api.Mapping.AutoMapper;
using demo_web_api.Validation.Validators.Company;
using demo_web_api.Validation.Validators.Employee;
using demo_web_api.Validation.Validators.Project;
using demo_web_api.Validation.Validators.ProjectEmployee;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Using SqlServer with connection string from appsettings
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adding AutoMapper to DI container
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Adding unit of work to DI container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Adding validators to DI container
builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<ProjectDto>, ProjectValidator>();
builder.Services.AddScoped<IValidator<CompanyDto>, CompanyValidator>();
builder.Services.AddScoped<IValidator<ProjectEmployeeDto>, ProjectEmployeeValidator>();

// Adding repositories to DI container
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();

// Adding services to DI container
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();

// Adding controllers, swagger and endpoints explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allowing front project to endpoints
builder.Services.AddCors(
    options => {
        options.AddDefaultPolicy(
            policy => {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Using cors (for frontend), mapping controllers and starting app
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();