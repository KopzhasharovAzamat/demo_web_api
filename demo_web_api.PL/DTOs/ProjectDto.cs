namespace demo_web_api.PL.DTOs;

public class ProjectDto {
    public Guid      Id                  { get; set; }
    public string    Name                { get; set; }
    public Guid      CustomerCompanyId   { get; set; }
    public Guid      ContractorCompanyId { get; set; }
    public DateTime  StartDate           { get; set; }
    public DateTime? EndDate             { get; set; }
    public int       Priority            { get; set; }
    public Guid?     ProjectManagerId    { get; set; }
}