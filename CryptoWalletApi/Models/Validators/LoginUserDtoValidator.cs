using CryptoWalletApi.Models;
using FluentValidation;

namespace CryptoWalletApi.Models.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty();

        RuleFor(u => u.Password)
            .NotEmpty();
    }
}
