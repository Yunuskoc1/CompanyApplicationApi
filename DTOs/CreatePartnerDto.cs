namespace CompanyApplicationApi.DTOs;

public class CreatePartnerDto
{
    public string IdentityNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}