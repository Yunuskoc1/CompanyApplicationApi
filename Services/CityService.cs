using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Repositories;

namespace CompanyApplicationApi.Services;

public class CityService(ICityRepository cityRepository) : ICityService
{
    public async Task<List<CityDto>> GetCitiesAsync()
    {
        var cities = await cityRepository.GetCitiesAsync();

        return cities.Select(c => new CityDto(c.Id, c.Name)).ToList();
    }

    public async Task<List<DistrictDto>> GetDistrictsByCityIdAsync(int cityId)
    {
        var districts = await cityRepository.GetDistrictsByCityIdAsync(cityId);

        if (districts == null || districts.Count == 0)
        {
            return new List<DistrictDto>(); 
        }

        return districts.Select(d => new DistrictDto(d.Id, d.Name, d.CityId)).ToList();
    }
}