namespace demo_web_api.DTOs.ProjectEmployee;

public class ProjectEmployeeVm {
    public Guid      EmployeeId  { get; set; }
    public string    FullName    { get; set; }
    public string    Email       { get; set; }
    public Guid      ProjectId   { get; set; }
    public string    ProjectName { get; set; }
    public DateTime  StartDate   { get; set; }
    public DateTime? EndDate     { get; set; }
}