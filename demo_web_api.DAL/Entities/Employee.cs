namespace demo_web_api.DAL.Entities;

public class Employee {
    public int                    Id               { get; set; }
    public string                 FirstName        { get; set; }
    public string                 LastName         { get; set; }
    public string                 MiddleName       { get; set; }
    public string                 Email            { get; set; }
    public List<ProjectEmployees> ProjectEmployees { get; set; }
}