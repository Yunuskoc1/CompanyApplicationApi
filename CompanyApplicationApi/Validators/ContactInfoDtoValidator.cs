using CompanyApplicationApi.DTOs;
using FluentValidation;

namespace CompanyApplicationApi.Validators;

public class ContactInfoDtoValidator : AbstractValidator<CreateContactInfoDto>
{
    public ContactInfoDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.");
    }
}