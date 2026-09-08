namespace CompanyApplicationApi.DTOs;

public class PartnerListDto
{
    public Guid Id { get; set; }
    public string IdentityNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
}
    
