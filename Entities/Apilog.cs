namespace CompanyApplicationApi.Entities;

public class ApiLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string CorrelationId { get; set; } = string.Empty;
    
    public string? ProcessId { get; set; }
    
    public string Path { get; set; } = string.Empty; 
    public string Method { get; set; } = string.Empty; 
    public string RequestBody { get; set; } = string.Empty; 
    public string ResponseBody { get; set; } = string.Empty; 
    public int StatusCode { get; set; } 
    public long ExecutionTimeMs { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}