using FluentValidation;

namespace CryptoWalletApi.Models.Validators;

public class MoneyOperationsWalletDtoValidator : AbstractValidator<MoneyOperationsWalletDto>
{
    public MoneyOperationsWalletDtoValidator()
    {
        RuleFor(w => w.Amount)
            .GreaterThan(0);

        RuleFor(w => w.Password)
            .NotEmpty();
    }
}
