using System.Globalization;
using CompanyApplicationApi.Data;
using CompanyApplicationApi.DTOs;
using FluentValidation;

namespace CompanyApplicationApi.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");

    public AddressDtoValidator(AppDbContext dbContext)
    {
        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter uzunluğunda olmalıdır")
            .Must((provinceName) =>
            {
                if (string.IsNullOrWhiteSpace(provinceName)) return false;
                
                var formattedInput = provinceName.Trim().ToLower(TrCulture);
                return dbContext.Cities.ToList().Any(c => c.Name.Trim().ToLower(TrCulture) == formattedInput);
            }).WithMessage("Girdiğiniz il (şehir) bulunamamaktadır.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter uzunluğunda olmalıdır")
            .Must((dto, districtName) =>
            {
                if (string.IsNullOrWhiteSpace(districtName) || string.IsNullOrWhiteSpace(dto.Province)) return false;

                var formattedProvince = dto.Province.Trim().ToLower(TrCulture);
                var formattedDistrict = districtName.Trim().ToLower(TrCulture);

                return dbContext.Districts.ToList().Any(d => 
                    d.Name.Trim().ToLower(TrCulture) == formattedDistrict && 
                    d.City.Name.Trim().ToLower(TrCulture) == formattedProvince);
            }).WithMessage("Girdiğiniz ilçe, seçilen il sınırları içerisinde bulunamamaktadır.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter uzunluğunda olmalıdır");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .Length(5).WithMessage("Tam 5 karakter uzunluğunda olmalıdır")
            .Matches(@"^\d+$").WithMessage("Özel karakter içermemelidir");

        RuleFor(x => x.FullAddress)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MinimumLength(10).WithMessage("Minimum 10 karakter uzunluğunda olmalıdır")
            .MaximumLength(500).WithMessage("Maksimum 500 karakter uzunluğunda olmalıdır");
    }
}