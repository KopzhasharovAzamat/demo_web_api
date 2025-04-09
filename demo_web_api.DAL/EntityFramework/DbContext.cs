using demo_web_api.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace demo_web_api.DAL.EntityFramework;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project>         Projects         { get; set; }
    public DbSet<Employee>        Employees        { get; set; }
    public DbSet<Company>         Companies        { get; set; }
    public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectEmployee>()
            .HasKey(x => new { x.ProjectId, x.EmployeeId });

        modelBuilder.Entity<ProjectEmployee>()
            .HasOne(x => x.Project)
            .WithMany(x => x.ProjectEmployees)
            .HasForeignKey(x => x.ProjectId);

        modelBuilder.Entity<ProjectEmployee>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.ProjectEmployees)
            .HasForeignKey(x => x.EmployeeId);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.CustomerCompany)
            .WithMany()
            .HasForeignKey(p => p.CustomerCompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.ContractorCompany)
            .WithMany()
            .HasForeignKey(p => p.ContractorCompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}