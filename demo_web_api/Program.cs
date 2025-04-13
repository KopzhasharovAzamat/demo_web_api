using demo_web_api.BLL.AutoMapper;
using demo_web_api.BLL.Interfaces;
using demo_web_api.BLL.Services;
using demo_web_api.BLL.Validation;
using demo_web_api.BLL.Validation.CompanyValidators;
using demo_web_api.BLL.Validation.EmployeeValidators;
using demo_web_api.BLL.Validation.ProjectEmployeeValidators;
using demo_web_api.BLL.Validation.ProjectValidators;
using demo_web_api.DAL.Entities;
using demo_web_api.DAL.EntityFramework;
using demo_web_api.DAL.Interfaces;
using demo_web_api.DAL.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Using SqlServer with connection string from appsettings
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adding AutoMapper to DI container
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Adding repositories and unit of work to DI container
builder.Services.AddRepositories();

// Adding validators to DI container
builder.Services.AddValidators();

// Adding services to DI container
builder.Services.AddServices();

// Adding controllers, swagger and endpoints explorer
builder.Services.AddControllers().AddJsonOptions(
    options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; }
);
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