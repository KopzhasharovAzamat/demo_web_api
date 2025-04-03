namespace demo_web_api.DAL.Entities;

public class Project {
    public int                    Id                { get; set; }
    public string                 Name              { get; set; }
    public Company                CustomerCompany   { get; set; }
    public Company                ContractorCompany { get; set; }
    public DateTime               StartDate         { get; set; }
    public DateTime?              EndDate           { get; set; }
    public int                    Priority          { get; set; }
    public int?                   ProjectManagerId  { get; set; }
    public Employee               ProjectManager    { get; set; }
    public List<ProjectEmployees> ProjectEmployees  { get; set; }
}