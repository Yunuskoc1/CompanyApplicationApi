namespace CompanyApplicationApi.Entities;
public class Partners

{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string IdentityNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}