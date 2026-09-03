using System.Text.Json;
using CompanyApplicationApi.Entities;

namespace CompanyApplicationApi.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Cities.Any())
        {
            var cityJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "City.json");
            if (File.Exists(cityJsonPath))
            {
                var jsonString = await File.ReadAllTextAsync(cityJsonPath);
                using var doc = JsonDocument.Parse(jsonString);
                var cities = new List<City>();

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    if (element.TryGetProperty("name", out var tableName) && tableName.GetString() == "il")
                    {
                        if (element.TryGetProperty("data", out var dataArray))
                        {
                            foreach (var item in dataArray.EnumerateArray())
                            {
                                var idStr = item.GetProperty("id").GetString();
                                var name = item.GetProperty("name").GetString();

                                if (int.TryParse(idStr, out int id) && !string.IsNullOrEmpty(name))
                                {
                                    cities.Add(new City { Id = id, Name = name });
                                }
                            }
                        }
                    }
                }

                if (cities.Count > 0)
                {
                    await context.Cities.AddRangeAsync(cities);
                    await context.SaveChangesAsync();
                }
            }
        }
        if (!context.Districts.Any())
        {
            var districtJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "District.json");
            if (File.Exists(districtJsonPath))
            {
                var jsonString = await File.ReadAllTextAsync(districtJsonPath);
                using var doc = JsonDocument.Parse(jsonString);
                var districts = new List<District>();

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    if (element.TryGetProperty("name", out var tableName) && tableName.GetString() == "ilce")
                    {
                        if (element.TryGetProperty("data", out var dataArray))
                        {
                            foreach (var item in dataArray.EnumerateArray())
                            {
                                var idStr = item.GetProperty("id").GetString();
                                var cityIdStr = item.GetProperty("il_id").GetString();
                                var name = item.GetProperty("name").GetString();

                                if (int.TryParse(idStr, out int id) &&
                                    int.TryParse(cityIdStr, out int cityId) &&
                                    !string.IsNullOrEmpty(name))
                                {
                                    districts.Add(new District
                                    {
                                        Id = id,
                                        CityId = cityId,
                                        Name = name
                                    });
                                }
                            }
                        }
                    }
                }

                if (districts.Count > 0)
                {
                    await context.Districts.AddRangeAsync(districts);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}