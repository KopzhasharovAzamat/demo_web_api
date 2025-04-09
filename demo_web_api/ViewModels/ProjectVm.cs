namespace demo_web_api.PL.DTOs;

public class ProjectVm
{
    public Guid      Id                    { get; set; }
    public string    Name                  { get; set; }
    public Guid      CustomerCompanyId     { get; set; }
    public string    CustomerCompanyName   { get; set; }
    public Guid      ContractorCompanyId   { get; set; }
    public string    ContractorCompanyName { get; set; }
    public DateTime  StartDate             { get; set; }
    public DateTime? EndDate               { get; set; }
    public int       Priority              { get; set; }
    public Guid?     ProjectManagerId      { get; set; }
    public string?   ProjectManagerName    { get; set; }
}
