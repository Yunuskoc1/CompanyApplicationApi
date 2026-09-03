namespace CompanyApplicationApi.DTOs;

public class PartnerDto
{
    public Guid Id { get; set; }
    public string IdentityNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}