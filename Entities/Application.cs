using CompanyApplicationApi.Enums;

namespace CompanyApplicationApi.Entities;

public class Application
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProcessId { get; set; } = Guid.NewGuid();
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public ContactInfo? ContactInfo { get; set; }
    public CompanyInfo? CompanyInfo { get; set; }
    public Address? Address { get; set; }
    public ICollection<Partners> Partners { get; set; } = new List<Partners>();
}