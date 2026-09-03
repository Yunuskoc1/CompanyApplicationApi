namespace CompanyApplicationApi.DTOs;

public class ApplicationResponseDto
{
    public Guid ProcessId { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ContactInfoDto? Contact { get; set; }
    public CompanyInfoDto? Company { get; set; }
    public AddressDto? Address { get; set; }
    public List<PartnerDto> Partners { get; set; } = new();
}