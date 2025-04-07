namespace demo_web_api.ViewModels;

public class ProjectEmployeeVm {
    public Guid   ProjectId         { get; set; }
    public string ProjectName       { get; set; }
    public Guid   EmployeeId        { get; set; }
    public string EmployeeLastName  { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeEmail     { get; set; }
}