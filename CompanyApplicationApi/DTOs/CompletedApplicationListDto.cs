namespace CompanyApplicationApi.DTOs;

public class CompletedApplicationListDto
{
    public Guid ProcessId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }
}