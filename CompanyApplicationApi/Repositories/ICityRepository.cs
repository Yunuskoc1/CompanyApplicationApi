using CompanyApplicationApi.Entities;

namespace CompanyApplicationApi.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetCitiesAsync();
    Task<List<District>> GetDistrictsByCityIdAsync(int cityId);
}