using CryptoWalletApi.Entities;
using FluentValidation;

namespace CryptoWalletApi.Models.Validators;

public class SendMoneyOperationsWalletDtoValidator : AbstractValidator<SendMoneyOperationsWalletDto>
{
    private readonly CryptoWalletDbContext _dbContext;

    public SendMoneyOperationsWalletDtoValidator(CryptoWalletDbContext dbContext)
    {
        _dbContext = dbContext;
        RuleFor(w => w.Password)
            .NotEmpty();

        RuleFor(w => w.Amount)
            .GreaterThan(0);

        RuleFor(w => w.DestinationGuid)
            .NotEmpty();
    }
}
