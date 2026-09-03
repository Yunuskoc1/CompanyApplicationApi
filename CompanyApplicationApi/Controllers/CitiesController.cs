using CompanyApplicationApi.DTOs;
using CompanyApplicationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApplicationApi.Controllers;

[ApiController]
public class CitiesController(ICityService cityService) : ControllerBase
{
    [HttpGet("api/cities")]
    public async Task<ActionResult<List<CityDto>>> GetCities()
    {
        var cities = await cityService.GetCitiesAsync();
        return Ok(cities);
    }

    [HttpGet("api/cities/{cityId:int}/districts")]
    public async Task<ActionResult<List<DistrictDto>>> GetDistrictsByCityId(int cityId)
    {
        var districts = await cityService.GetDistrictsByCityIdAsync(cityId);
        return Ok(districts);
    }
}