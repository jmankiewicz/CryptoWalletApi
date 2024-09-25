using CryptoWalletApi.Entities;
using CryptoWalletApi.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CryptoWalletApi.Models.Validators;

public class CreateWalletDtoValidator : AbstractValidator<CreateWalletDto>
{
    private readonly CryptoWalletDbContext _dbContext;
    private readonly IUserContextService _userContextService;

    public CreateWalletDtoValidator(CryptoWalletDbContext dbContext, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;

        RuleFor(c => c.WalletName)
            .Length(4, 12)
            .WithMessage("WalletName field length must be between 4 - 12 characters.")
            .Custom((name, context) =>
            {
                var takenName = _dbContext
                    .Wallets
                    .Where(w => w.UserId == _userContextService.GetUserId)
                    .Any(w => w.WalletName == name);

                if (takenName)
                {
                    context.AddFailure($"You already have wallet with name: {name}, pick another.");
                }
            });

        RuleFor(c => c.CurrencyName)
            .Custom((currency, context) =>
            {
                var currencies = _dbContext
                    .Currencies
                    .AsNoTracking()
                    .Select(c => c.Name.ToLower())
                    .ToList();

                if (!currencies.Any(c => c == currency.ToLower()))
                {
                    context.AddFailure($"Please select proper currency: {string.Join(", ", currencies)}.");
                }
            });

        RuleFor(c => c.Password)
            .Equal(c => c.ConfirmPassword)
            .WithMessage("Password and ConfirmPassword fields must be the same.");
    }
}
