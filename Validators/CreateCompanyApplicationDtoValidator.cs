using CompanyApplicationApi.DTOs;
using FluentValidation;

namespace CompanyApplicationApi.Validators;

public class CreateCompanyDtoValidator : AbstractValidator<CreatePartnerDto>
{
    public CreateCompanyDtoValidator()
    {
        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .Must(CompanyInfoDtoValidator.BeValidTcOrVkn)
            .WithMessage("Geçerli bir TC Kimlik No (11 hane) veya Vergi No (10 hane) giriniz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .MinimumLength(2).WithMessage("Minimum 2 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter uzunluğunda olmalıdır");

        RuleFor(x => x.SharePercentage)
            .NotNull().WithMessage("Bu alan boş bırakılamaz")
            .GreaterThan(0).WithMessage("Hisse %0'dan büyük olmalı")
            .LessThanOrEqualTo(100).WithMessage("Hisse en fazla %100 olabilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz")
            .EmailAddress().WithMessage("Geçerli bir E-posta adresi giriniz");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Bu alan boş bırakılamaz")
            .Length(11).WithMessage("Numara 11 haneli olmalıdır");
    }
}