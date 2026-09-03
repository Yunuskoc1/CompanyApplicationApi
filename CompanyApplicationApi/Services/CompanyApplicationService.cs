using CompanyApplicationApi.Data; 
using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Entities;
using CompanyApplicationApi.Repositories;
using Microsoft.EntityFrameworkCore;
using CompanyApplicationApi.Enums;

namespace CompanyApplicationApi.Services;

public class CompanyApplicationService : ICompanyApplicationService
{
    private readonly ICompanyApplicationRepository _repository;
    private readonly AppDbContext _context;

    public CompanyApplicationService(ICompanyApplicationRepository repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Guid> StartApplicationAsync()
    {
        var application = new Application
        {
            Id = Guid.NewGuid(),
            ProcessId = Guid.NewGuid(),
            Status = ApplicationStatus.Draft,
            CurrentStep = 1,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddApplicationAsync(application);
        await _repository.SaveChangesAsync();

        return application.ProcessId;
    }

    public async Task<bool> SaveContactInfoAsync(Guid processId, CreateContactInfoDto dto)
    {
        var app = await _repository.GetByProcessIdAsync(processId);
        if (app == null) return false;

        var existingContact = await _repository.GetContactInfoByApplicationIdAsync(app.Id);

        if (existingContact != null)
        {
            existingContact.Email = dto.Email;
            existingContact.PhoneNumber = dto.PhoneNumber;
        }
        else
        {
            var contact = new ContactInfo
            {
                Id = Guid.NewGuid(),
                ApplicationId = app.Id,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            await _repository.AddContactInfoAsync(contact);
        }

        app.CurrentStep = 2;
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveCompanyInfoAsync(Guid processId, CreateCompanyInfoDto dto)
    {
        var app = await _repository.GetByProcessIdAsync(processId);
        if (app == null) return false;

        var company = new CompanyInfo
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            IdentityNumber = dto.IdentityNumber,
            CompanyName = dto.CompanyName,
            YearsInSector = dto.YearsInSector,
            Iban = dto.Iban,
            Email = dto.Email,
            Website = dto.Website
        };

        await _repository.AddCompanyInfoAsync(company);
        app.CurrentStep = 3;
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<PartnerListDto>> GetPartnersAsync(Guid processId)
    {
        var partners = await _repository.GetPartnersByProcessIdAsync(processId);
        return partners.Where(p => !p.IsDeleted).Select(p => new PartnerListDto
        {
            Id = p.Id,
            IdentityNumber = p.IdentityNumber,
            Name = p.Name,
            SharePercentage = p.SharePercentage
        }).ToList();
    }

    public async Task<PartnerDetailDto?> GetPartnerByIdAsync(Guid processId, Guid partnerId)
    {
        var partner = await _repository.GetPartnerByIdAsync(processId, partnerId);
        if (partner == null || partner.IsDeleted) return null;

        return new PartnerDetailDto
        {
            Id = partner.Id,
            IdentityNumber = partner.IdentityNumber,
            Name = partner.Name,
            SharePercentage = partner.SharePercentage,
            Email = partner.Email,
            PhoneNumber = partner.PhoneNumber
        };
    }

    public async Task<Guid> AddPartnerAsync(Guid processId, CreatePartnerDto dto)
    {
        var app = await _repository.GetByProcessIdAsync(processId);
        if (app == null) throw new KeyNotFoundException("Başvuru bulunamadı.");

        var partner = new Partners
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            IdentityNumber = dto.IdentityNumber,
            Name = dto.Name,
            SharePercentage = dto.SharePercentage,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsDeleted = false
        };

        await _repository.AddPartnerAsync(partner);
        app.CurrentStep = 4;
        await _repository.SaveChangesAsync();
        return partner.Id;
    }

    public async Task<bool> UpdatePartnerAsync(Guid processId, Guid partnerId, UpdatePartnerDto dto)
    {
        var partner = await _repository.GetPartnerByIdAsync(processId, partnerId);
        if (partner == null || partner.IsDeleted) return false;

        partner.IdentityNumber = dto.IdentityNumber;
        partner.Name = dto.Name;
        partner.SharePercentage = dto.SharePercentage;
        partner.Email = dto.Email;
        partner.PhoneNumber = dto.PhoneNumber;

        await _repository.UpdatePartnerAsync(partner);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePartnerAsync(Guid processId, Guid partnerId)
    {
        var partner = await _repository.GetPartnerByIdAsync(processId, partnerId);
        if (partner == null || partner.IsDeleted) return false;

        partner.IsDeleted = true;
        partner.DeletedAt = DateTime.UtcNow;

        await _repository.DeletePartnerAsync(partner);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveAddressInfoAsync(Guid processId, AddressDto dto)
    {
        var app = await _repository.GetByProcessIdAsync(processId);
        if (app == null) return false;
        var existingAddress = await _repository.GetAddressByApplicationIdAsync(app.Id);

        if (existingAddress != null)
        {
            existingAddress.Province = dto.Province;
            existingAddress.District = dto.District;
            existingAddress.Neighborhood = dto.Neighborhood;
            existingAddress.PostalCode = dto.PostalCode;
            existingAddress.FullAddress = dto.FullAddress;
        }
        else
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                ApplicationId = app.Id,
                Province = dto.Province,
                District = dto.District,
                Neighborhood = dto.Neighborhood,
                PostalCode = dto.PostalCode,
                FullAddress = dto.FullAddress
            };
            await _repository.AddAddressAsync(address);
        }

        app.CurrentStep = 5;
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteApplicationAsync(Guid processId)
    {
        var app = await _repository.GetByProcessIdAsync(processId);
        if (app == null) return false;

        app.Status = ApplicationStatus.Completed;
        app.CompletedAt = DateTime.UtcNow;

        await _repository.CompleteApplicationAsync(app);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<CompletedApplicationListDto>> GetCompletedApplicationsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _repository.GetCompletedApplicationsAsync(startDate, endDate);
    }

    public async Task<ApplicationResponseDto?> GetCompletedApplicationDetailAsync(Guid processId)
    {
        var app = await _repository.GetCompletedApplicationDetailAsync(processId);
        if (app == null || app.Status != ApplicationStatus.Completed) return null;

        int cityId = 0;
        int? districtId = null;

        if (app.Address != null)
        {
            var trCulture = new System.Globalization.CultureInfo("tr-TR");
            var searchProvince = app.Address.Province.Trim().ToLower(trCulture);
            var searchDistrict = app.Address.District.Trim().ToLower(trCulture);

            var cities = await _context.Cities.ToListAsync();
            var city = cities.FirstOrDefault(c => c.Name.Trim().ToLower(trCulture) == searchProvince);

            if (city != null)
            {
                cityId = city.Id;

                var districts = await _context.Districts
                    .Where(d => d.CityId == city.Id)
                    .ToListAsync();

                var district = districts.FirstOrDefault(d => d.Name.Trim().ToLower(trCulture) == searchDistrict);

                if (district != null)
                {
                    districtId = district.Id;
                }
            }
        }

        return new ApplicationResponseDto
        {
            ProcessId = app.ProcessId,
            CompletedAt = app.CompletedAt,
            Contact = app.ContactInfo != null
                ? new ContactInfoDto
                {
                    Email = app.ContactInfo.Email,
                    PhoneNumber = app.ContactInfo.PhoneNumber
                }
                : null,
            Company = app.CompanyInfo != null
                ? new CompanyInfoDto
                {
                    CompanyName = app.CompanyInfo.CompanyName,
                    Iban = app.CompanyInfo.Iban,
                    Email = app.CompanyInfo.Email,
                    IdentityNumber = app.CompanyInfo.IdentityNumber,
                    YearsInSector = app.CompanyInfo.YearsInSector,
                    Website = app.CompanyInfo.Website
                }
                : null,
            Address = app.Address != null
                ? new AddressDto
                {
                    CityId = cityId,
                    DistrictId = districtId,
                    Province = app.Address.Province,
                    District = app.Address.District,
                    Neighborhood = app.Address.Neighborhood,
                    PostalCode = app.Address.PostalCode,
                    FullAddress = app.Address.FullAddress
                }
                : null,
            Partners = app.Partners?.Select(p => new PartnerDto
            {
                Id = p.Id,
                IdentityNumber = p.IdentityNumber,
                Name = p.Name,
                SharePercentage = p.SharePercentage,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber
            }).ToList() ?? new List<PartnerDto>()
        };
    }

    public async Task<IEnumerable<PartnerDto>> GetPartnersByProcessIdAsync(Guid processId)
    {
        var partners = await _repository.GetPartnersByProcessIdAsync(processId);

        return partners.Select(p => new PartnerDto
        {
            Id = p.Id,
            IdentityNumber = p.IdentityNumber,
            Name = p.Name,
            SharePercentage = p.SharePercentage,
            Email = p.Email,
            PhoneNumber = p.PhoneNumber
        });
    }
}