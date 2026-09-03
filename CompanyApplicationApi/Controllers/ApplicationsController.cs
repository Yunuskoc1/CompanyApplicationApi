using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApplicationApi.Controllers;

[ApiController]
[Route("api/applications")]
public class ApplicationsController(ICompanyApplicationService service) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartApplication()
    {
        var processId = await service.StartApplicationAsync();
        return Ok(new { processId });
    }

    [HttpPost("{processId}/contact")]
    public async Task<IActionResult> SaveContactInfo(Guid processId, [FromBody] CreateContactInfoDto dto)
    {
        var result = await service.SaveContactInfoAsync(processId, dto);
        if (!result) return NotFound(new { message = "Başvuru bulunamadı." });
        return Ok(new { message = "İletişim bilgileri kaydedildi." });
    }

    [HttpPost("{processId}/company")]
    public async Task<IActionResult> SaveCompanyInfo(Guid processId, [FromBody] CreateCompanyInfoDto dto)
    {
        var result = await service.SaveCompanyInfoAsync(processId, dto);
        if (!result) return NotFound(new { message = "Başvuru bulunamadı." });
        return Ok(new { message = "Şirket bilgileri kaydedildi." });
    }

    [HttpGet("{processId}/partners")]
    public async Task<IActionResult> GetPartners(Guid processId)
    {
        var partners = await service.GetPartnersAsync(processId);
        return Ok(partners);
    }

    [HttpGet("{processId}/partners/{partnerId}")]
    public async Task<IActionResult> GetPartnerDetail(Guid processId, Guid partnerId)
    {
        var partner = await service.GetPartnerByIdAsync(processId, partnerId);
        if (partner == null) return NotFound(new { message = "Ortak bulunamadı." });
        return Ok(partner);
    }

    [HttpPost("{processId}/partners")]
    public async Task<IActionResult> AddPartner(Guid processId, [FromBody] CreatePartnerDto dto)
    {
        var partnerId = await service.AddPartnerAsync(processId, dto);

        if (partnerId == Guid.Empty)
        {
            return NotFound(new { message = "Başvuru bulunamadı." });
        }
        
        return Ok(new
        {
            message = "Ortak başarıyla eklendi.",
            partnerId = partnerId
        });
    }

    [HttpPut("{processId}/partners/{partnerId}")]
    public async Task<IActionResult> UpdatePartner(Guid processId, Guid partnerId, [FromBody] UpdatePartnerDto dto)
    {
        var result = await service.UpdatePartnerAsync(processId, partnerId, dto);
        if (!result) return NotFound(new { message = "Ortak bulunamadı." });
        return Ok(new { message = "Ortak bilgileri güncellendi." });
    }

    [HttpDelete("{processId}/partners/{partnerId}")]
    public async Task<IActionResult> DeletePartner(Guid processId, Guid partnerId)
    {
        var result = await service.DeletePartnerAsync(processId, partnerId);
        if (!result) return NotFound(new { message = "Ortak bulunamadı." });
        return Ok(new { message = "Ortak başarıyla silindi." });
    }

    [HttpPost("{processId}/address")]
    public async Task<IActionResult> SaveAddress(Guid processId, [FromBody] AddressDto dto)
    {
        var result = await service.SaveAddressInfoAsync(processId, dto);
        if (!result) return NotFound(new { message = "Başvuru bulunamadı." });
        return Ok(new { message = "Adres bilgileri kaydedildi." });
    }

    [HttpPost("{processId}/complete")]
    public async Task<IActionResult> CompleteApplication(Guid processId)
    {
        var result = await service.CompleteApplicationAsync(processId);
        if (!result) return NotFound(new { message = "Başvuru bulunamadı." });
        return Ok(new { message = "Akış başarıyla tamamlandı." });
    }

    [HttpGet]
    public async Task<IActionResult> GetCompletedApplications([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var applications = await service.GetCompletedApplicationsAsync(startDate, endDate);
        return Ok(applications);
    }

    [HttpGet("{processId}")]
    public async Task<IActionResult> GetCompletedApplicationDetail(Guid processId)
    {
        var detail = await service.GetCompletedApplicationDetailAsync(processId);
        if (detail == null) return NotFound(new { message = "Başvuru bulunamadı veya henüz tamamlanmadı." });
        return Ok(detail);
    }

    [HttpGet("{processId}/partners/all")]
    public async Task<IActionResult> GetApplicationPartners(Guid processId)
    {
        var partners = await service.GetPartnersByProcessIdAsync(processId);
        return Ok(partners);
    }
}