namespace demo_web_api.DAL.Entities;

public class ProjectEmployee {
    public int      EmployeeId { get; set; }
    public Employee Employee   { get; set; }
    public int      ProjectId  { get; set; }
    public Project  Project    { get; set; }
}