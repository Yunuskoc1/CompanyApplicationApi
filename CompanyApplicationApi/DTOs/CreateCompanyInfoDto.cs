namespace CompanyApplicationApi.DTOs;

public class CreateCompanyInfoDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Iban { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public int YearsInSector { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Website { get; set; }
}



