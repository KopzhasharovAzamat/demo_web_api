﻿namespace demo_web_api.DTOs.ProjectEmployee;

public class AssignEmployeesDto {
    public Guid       ProjectId   { get; set; }
    public List<Guid> EmployeeIds { get; set; } = new();
}