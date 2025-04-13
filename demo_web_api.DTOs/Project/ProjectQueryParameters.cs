namespace demo_web_api.DTOs.Project;

public class ProjectQueryParameters {
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo   { get; set; }
    public int?      MinPriority   { get; set; }
    public int?      MaxPriority   { get; set; }
    public string?   SortBy        { get; set; } = "name"; // default
    public bool      Descending    { get; set; } = false;
}