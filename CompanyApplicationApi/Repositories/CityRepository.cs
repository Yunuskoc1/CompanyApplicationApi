using CompanyApplicationApi.Data;
using CompanyApplicationApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyApplicationApi.Repositories;

public class CityRepository(AppDbContext context) : ICityRepository
{
    public async Task<List<City>> GetCitiesAsync()
    {
        return await context.Cities
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<District>> GetDistrictsByCityIdAsync(int cityId)
    {
        return await context.Districts
            .Where(d => d.CityId == cityId)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }
}