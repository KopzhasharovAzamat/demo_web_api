namespace demo_web_api.DAL.Entities;

public class Project {
    public Guid                  Id                  { get; set; }
    public string                Name                { get; set; }
    public Guid                  CustomerCompanyId   { get; set; }
    public Company               CustomerCompany     { get; set; }
    public Guid                  ContractorCompanyId { get; set; }
    public Company               ContractorCompany   { get; set; }
    public DateTime              StartDate           { get; set; }
    public DateTime?             EndDate             { get; set; }
    public int                   Priority            { get; set; }
    public Guid?                 ProjectManagerId    { get; set; }
    public Employee              ProjectManager      { get; set; }
    public List<ProjectEmployee> ProjectEmployees    { get; set; }
}