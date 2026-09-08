using CompanyApplicationApi.Data;
using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Entities;
using CompanyApplicationApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace CompanyApplicationApi.Repositories;

public class CompanyApplicationRepository : ICompanyApplicationRepository
{
    private readonly AppDbContext _context;

    public CompanyApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Application?> GetByProcessIdAsync(Guid processId)
    {
        return await _context.Applications
            .Include(a => a.ContactInfo)
            .Include(a => a.CompanyInfo)
            .Include(a => a.Address)
            .Include(a => a.Partners.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(a => a.ProcessId == processId);
    }

    public async Task AddAsync(Application application)
    {
        await _context.Applications.AddAsync(application);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddContactInfoAsync(ContactInfo contactInfo)
    {
        await _context.ContactInfos.AddAsync(contactInfo);
    }

    public async Task AddCompanyInfoAsync(CompanyInfo companyInfo)
    {
        await _context.CompanyInfos.AddAsync(companyInfo);
    }

    public async Task AddPartnerAsync(Partners partners)
    {
        await _context.Partners.AddAsync(partners);
    }

    public async Task<Partners?> GetPartnerByIdAsync(Guid partnerId)
    {
        return await _context.Partners.FirstOrDefaultAsync(p => p.Id == partnerId);
    }

    public async Task<List<Partners>> GetPartnersByProcessIdAsync(Guid processId)
    {
        return await _context.Applications
            .Where(a => a.ProcessId == processId)
            .SelectMany(a => a.Partners)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<Partners?> GetPartnerByIdAsync(Guid processId, Guid partnerId)
    {
        var applicationId = await _context.Applications
            .Where(a => a.ProcessId == processId)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (applicationId == Guid.Empty)
            return null;

        return await _context.Partners
            .FirstOrDefaultAsync(p => p.Id == partnerId
                                      && p.ApplicationId == applicationId
                                      && !p.IsDeleted);
    }

    public async Task UpdatePartnerAsync(Partners partners)
    {
        _context.Partners.Update(partners);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePartnerAsync(Partners partners)
    {
        partners.IsDeleted = true;
        _context.Partners.Update(partners);
        await _context.SaveChangesAsync();
    }

    public async Task AddAddressAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteApplicationAsync(Application application)
    {
        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CompletedApplicationListDto>> GetCompletedApplicationsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Applications
            .Where(a => a.Status == ApplicationStatus.Completed)
            .AsQueryable();

        if (startDate.HasValue)
        {
            var startUtc = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            query = query.Where(a => a.CompletedAt >= startUtc);
        }

        if (endDate.HasValue)
        {
            var endUtc = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            query = query.Where(a => a.CompletedAt <= endUtc);
        }

        return await query
            .OrderByDescending(a => a.CompletedAt)
            .Select(a => new CompletedApplicationListDto
            {
                ProcessId = a.ProcessId,
                CompanyName = a.CompanyInfo != null ? a.CompanyInfo.CompanyName : string.Empty,
                CompletedAt = a.CompletedAt
            })
            .ToListAsync();
    }
    public async Task<Application?> GetCompletedApplicationDetailAsync(Guid processId)
    {
        return await _context.Applications
            .Include(a => a.ContactInfo)
            .Include(a => a.CompanyInfo)
            .Include(a => a.Address)
            .Include(a => a.Partners.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(a => a.ProcessId == processId && a.Status == ApplicationStatus.Completed);
    }

    public async Task AddApplicationAsync(Application application)
    {
        await _context.Applications.AddAsync(application);
    }

    public async Task<Address?> GetAddressByApplicationIdAsync(Guid applicationId)
    {
        return await _context.Addresses.FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
    }

    public async Task<ContactInfo?> GetContactInfoByApplicationIdAsync(Guid applicationId)
    {
        return await _context.ContactInfos.FirstOrDefaultAsync(c => c.ApplicationId == applicationId);
    }
}