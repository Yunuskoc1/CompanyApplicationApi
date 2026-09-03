namespace CompanyApplicationApi.DTOs;

public class CompanyInfoDto
{
    public string IdentityNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; }= string.Empty;
    public int YearsInSector { get; set; }
    public string Iban { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Website { get; set; } 
}