namespace demo_web_api.DTOs.ProjectEmployee;

public class ProjectEmployeeDto {
    public Guid ProjectId         { get; set; }
    public Guid EmployeeId        { get; set; }
    public bool ValidateForDelete { get; set; } = false;
}