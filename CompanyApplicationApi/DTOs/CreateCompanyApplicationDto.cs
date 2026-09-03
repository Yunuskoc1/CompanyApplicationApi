namespace CompanyApplicationApi.DTOs;

public class CreateCompanyApplicationDto
{
    public CompanyInfoDto CompanyInfo { get; set; } = null!;
    public ContactInfoDto ContactInfo { get; set; } = null!;
    public AddressDto Address { get; set; } = null!;
    public List<CreatePartnerDto> Partner { get; set; }  = new();
}