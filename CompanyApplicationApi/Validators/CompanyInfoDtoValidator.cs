using CompanyApplicationApi.DTOs;
using FluentValidation;

namespace CompanyApplicationApi.Validators;

public class CompanyInfoDtoValidator : AbstractValidator<CreateCompanyInfoDto>
{
    public CompanyInfoDtoValidator()
    {
        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .Must(BeValidTcOrVkn).WithMessage("Geçerli bir TC Kimlik No (11 hane) veya Vergi No (10 hane) giriniz.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MaximumLength(200).WithMessage("Maksimum 200 karakter uzunluğunda olmalı");

        RuleFor(x => x.YearsInSector)
            .NotNull().WithMessage("Bu alan boş bırakılamaz")
            .GreaterThanOrEqualTo(0).WithMessage("0 veya daha büyük olmalı");

        RuleFor(x => x.Iban)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .Must(BeValidIban).WithMessage("Geçersiz IBAN formatı! 'TR' ile başlamalı ve geçerli bir IBAN olmalıdır.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz")
            .EmailAddress().WithMessage("Geçerli bir E-posta adresi giriniz");

        RuleFor(x => x.Website)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage("Geçerli bir web sitesi adresi giriniz (ör: https://sirketiniz.com).");
    }

    public static bool BeValidTcOrVkn(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        if (value.Length == 11) return IsValidTckn(value);
        if (value.Length == 10) return IsValidVkn(value);

        return false;
    }

    public static bool IsValidTckn(string tckn)
    {
        if (tckn.Length != 11 || !tckn.All(char.IsDigit) || tckn.StartsWith("0"))
            return false;

        int[] digits = tckn.Select(c => c - '0').ToArray();

        int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
        int evenSum = digits[1] + digits[3] + digits[5] + digits[7];

        int digit10 = ((oddSum * 7) - evenSum) % 10;
        if (digit10 < 0) digit10 += 10;

        int digit11 = (digits.Take(10).Sum()) % 10;

        return digits[9] == digit10 && digits[10] == digit11;
    }

    public static bool IsValidVkn(string vkn)
    {
        if (vkn.Length != 10 || !vkn.All(char.IsDigit))
            return false;

        int[] digits = vkn.Select(c => c - '0').ToArray();
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            int v = (digits[i] + 9 - i) % 10;
            if (v != 0)
            {
                int p = (v * (int)Math.Pow(2, 9 - i)) % 9;
                sum += (p == 0) ? 9 : p;
            }
        }

        int lastDigit = (10 - (sum % 10)) % 10;
        return digits[9] == lastDigit;
    }

    public static bool BeValidIban(string iban)
    {
        if (string.IsNullOrWhiteSpace(iban)) return false;
        
        string cleanIban = iban.Replace(" ", "").Trim().ToUpper();

        if (cleanIban.Length != 26 || !cleanIban.StartsWith("TR")) return false;

        string asciiIban = cleanIban[4..] + cleanIban[..4];

        var sb = new System.Text.StringBuilder();
        foreach (char c in asciiIban)
        {
            if (char.IsLetter(c))
                sb.Append(c - 'A' + 10);
            else if (char.IsDigit(c))
                sb.Append(c);
            else
                return false; 
        }

        int checksum = 0;
        string numericString = sb.ToString();
        for (int i = 0; i < numericString.Length; i++)
        {
            checksum = (checksum * 10 + (numericString[i] - '0')) % 97;
        }

        return checksum == 1;
    }
}