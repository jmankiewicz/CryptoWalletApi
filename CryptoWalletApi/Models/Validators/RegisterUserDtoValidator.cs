using CryptoWalletApi.Entities;
using CryptoWalletApi.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly CryptoWalletDbContext _dbContext;


    public RegisterUserDtoValidator(CryptoWalletDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(u => u.Nickname)
            .Length(5, 26)
            .WithMessage("Nickname length must be between [5 - 26].");

        RuleFor(u => u.Email)
            .NotEmpty()
            .Custom((email, context) =>
            {
                var exists = _dbContext
                    .Users
                    .Any(u => u.Email == email);

                if (exists)
                {
                    context.AddFailure("This email already exists in database.");
                }
            });

        RuleFor(u => u.FullName)
            .NotEmpty()
            .WithMessage("FullName field cannot be empty.");

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber field cannot be empty.");

        RuleFor(u => u.Password)
            .Equal(u => u.ConfirmPassword)
            .WithMessage("Password and ConfirmPassword fields must be the same.");

        RuleFor(u => u.DateOfBirth)
            .Custom((dateOfBirth, context) =>
            {
                if(!(DateTime.Today.Year - dateOfBirth.Year >= 21))
                {
                    context.AddFailure("You must be over 21 years old to create an account.");
                }
            });
    }
}
