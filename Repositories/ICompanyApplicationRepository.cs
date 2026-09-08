using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Entities;
namespace CompanyApplicationApi.Repositories;


public interface ICompanyApplicationRepository
{
    Task<Application?> GetByProcessIdAsync(Guid processId);
    Task AddAsync(Application application);
    Task SaveChangesAsync();
    Task AddContactInfoAsync(ContactInfo contactInfo);
    
    Task AddCompanyInfoAsync(CompanyInfo companyInfo);
    
    Task AddPartnerAsync(Partners partners);
    Task<Partners?> GetPartnerByIdAsync(Guid partnerId);
    Task<List<Partners>> GetPartnersByProcessIdAsync(Guid processId);
    
    Task<Partners?> GetPartnerByIdAsync(Guid processId, Guid partnerId);
    
    Task UpdatePartnerAsync(Partners partners);
    
    Task DeletePartnerAsync(Partners partners);

    
    Task AddAddressAsync(Address address);
    
    Task CompleteApplicationAsync(Application application);

    Task<List<CompletedApplicationListDto>> GetCompletedApplicationsAsync(DateTime? startDate = null, DateTime? endDate = null);    
    Task<Application?> GetCompletedApplicationDetailAsync(Guid processId);
    
    Task AddApplicationAsync(Application application);
    
    Task<Address?> GetAddressByApplicationIdAsync(Guid applicationId);
    
    Task<ContactInfo?> GetContactInfoByApplicationIdAsync(Guid applicationId);
}