using CompanyApplicationApi.DTOs;

namespace CompanyApplicationApi.Services;

public interface ICompanyApplicationService
{
    Task<Guid> StartApplicationAsync();
    Task<bool> SaveContactInfoAsync(Guid processId, CreateContactInfoDto dto);
    Task<bool> SaveCompanyInfoAsync(Guid processId, CreateCompanyInfoDto dto);
    Task<List<PartnerListDto>> GetPartnersAsync(Guid processId);
    Task<PartnerDetailDto?> GetPartnerByIdAsync(Guid processId, Guid partnerId);
    Task<Guid> AddPartnerAsync(Guid processId, CreatePartnerDto dto);
    Task<bool> UpdatePartnerAsync(Guid processId, Guid partnerId, UpdatePartnerDto dto);
    Task<bool> DeletePartnerAsync(Guid processId, Guid partnerId);
    Task<bool> SaveAddressInfoAsync(Guid processId, AddressDto dto);
    Task<bool> CompleteApplicationAsync(Guid processId);
    Task<List<CompletedApplicationListDto>> GetCompletedApplicationsAsync(DateTime? startDate = null, DateTime? endDate = null);    Task<ApplicationResponseDto?> GetCompletedApplicationDetailAsync(Guid processId);
    Task<IEnumerable<PartnerDto>> GetPartnersByProcessIdAsync(Guid processId);
}