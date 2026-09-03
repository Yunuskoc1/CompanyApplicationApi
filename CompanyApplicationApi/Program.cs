using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using CompanyApplicationApi.Data;
using CompanyApplicationApi.Repositories;
using CompanyApplicationApi.Services;
using CompanyApplicationApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Repository & Service Dependency Injection
builder.Services.AddScoped<ICompanyApplicationRepository, CompanyApplicationRepository>();
builder.Services.AddScoped<ICompanyApplicationService, CompanyApplicationService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

// 3. Validation Rules Setup
builder.Services.AddValidatorsFromAssemblyContaining<CompanyInfoDtoValidator>();builder.Services.AddFluentValidationAutoValidation();

// 4. Controller & JSON Configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        
    });
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(new
        {
            message = "Validation failed",
            errors = errors
        });
    };
});

// 5. OpenAPI / Swagger Setup
builder.Services.AddOpenApi();

var app = builder.Build();

// 6. Pipeline & Middleware Setup
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // 1. Veritabanı ve Tablolar Yoksa Otomatik Oluştur
    await dbContext.Database.EnsureCreatedAsync();

    // 2. Tablo Kolon Güncellemesi
    await dbContext.Database.ExecuteSqlRawAsync(@"ALTER TABLE ""ContactInfos"" ADD COLUMN IF NOT EXISTS ""FirstName"" text;");
    
    // 3. Şehir ve İlçe Tohumlaması
    await DbInitializer.SeedAsync(dbContext);

}

// 8. Endpoints Routing
app.MapControllers();

app.Run();

// ============================================================================
// NOTLAR & YAPILANDIRMA (CONNECTION STRING)
// ============================================================================
// Projenin PostgreSQL / MSSQL bağlantı dizesini (ConnectionString) tanımlamak için:
// 
// 1. appsettings.json dosyası içerisine şu yapıyı ekleyebilirsiniz:
//    "ConnectionStrings": {
//        "DefaultConnection": "Host=localhost;Database=CompanyAppDb;Username=postgres;Password=your_password"
//    }
// 
// 2. Program.cs içerisinde DbContext servisine kaydederken kullanımı:
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseNpgsql(connectionString)); // (PostgreSQL için) veya UseSqlServer(connectionString)
// ============================================================================