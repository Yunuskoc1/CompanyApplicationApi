using CompanyApplicationApi.DTOs;

namespace CompanyApplicationApi.Services;

public interface ICityService
{
    Task<List<CityDto>> GetCitiesAsync();
    Task<List<DistrictDto>> GetDistrictsByCityIdAsync(int cityId);
}