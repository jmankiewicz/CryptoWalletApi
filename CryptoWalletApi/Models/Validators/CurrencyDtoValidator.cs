using FluentValidation;

namespace CryptoWalletApi.Models.Validators;

public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
{
    public CurrencyDtoValidator()
    {
        RuleFor(c => c.Name)
            .Length(2, 32);

        RuleFor(c => c.Code)
            .Length(1, 9);

        RuleFor(c => c.Value)
            .GreaterThan(0);
    }
}
