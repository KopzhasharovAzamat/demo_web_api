namespace demo_web_api.DTOs.Employee;

public class EmployeeVm {
    public Guid   Id         { get; set; }
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public string MiddleName { get; set; }
    public string Email      { get; set; }
}