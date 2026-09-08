namespace CompanyApplicationApi.Entities;

public class ContactInfo
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}