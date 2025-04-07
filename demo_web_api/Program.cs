using FluentValidation;
using demo_web_api.Validation;
using demo_web_api.BLL.Interfaces;
using demo_web_api.BLL.Services;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using demo_web_api.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<ProjectDto>, ProjectValidator>();
builder.Services.AddScoped<IValidator<CompanyDto>, CompanyValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();