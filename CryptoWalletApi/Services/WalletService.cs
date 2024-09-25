using AutoMapper;
using CryptoWalletApi.Authorization;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Middlewares.Exceptions;
using CryptoWalletApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CryptoWalletApi.Services;

public class WalletService
{
    private readonly CryptoWalletDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Wallet> _passwordHasher;
    private readonly IUserContextService _userContextService;
    private readonly IAuthorizationService _authorizationService;

    public WalletService(CryptoWalletDbContext dbContext, IMapper mapper, IPasswordHasher<Wallet> passwordHasher, IUserContextService userContextService, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _userContextService = userContextService;
        _authorizationService = authorizationService;
    }

    public async Task CreateWalletAsync(CreateWalletDto createWalletDto)
    {
        var currencies = await GetCurrenciesAsync();

        var wallet = new Wallet
        {
            WalletName = createWalletDto.WalletName,
            UserId = (int)_userContextService.GetUserId,
            Currency = currencies[createWalletDto.CurrencyName.ToLower()],
            WalletId = Guid.NewGuid(),
        };

        wallet.HashedPassword = _passwordHasher.HashPassword(wallet, createWalletDto.Password);

        await _dbContext
            .Wallets
            .AddAsync(wallet);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<WalletDto> OpenWalletAsync(int id, string password)
    {
        var wallet = await _dbContext
            .Wallets
            .Include(w => w.Currency)
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.Id == id);

        await VerifyWalletOwner(wallet);
        VerifyWalletPassword(wallet, password);

        var walletDto = _mapper.Map<WalletDto>(wallet);

        return walletDto;
    }

    public async Task ChargeWalletAsync(int id, MoneyOperationsWalletDto moneyOperationsWalletDto)
    {
        var wallet = await
            FirstWalletWithCurrencyAsync(w => w.Id == id);

        await VerifyWalletOwner(wallet);
        VerifyWalletPassword(wallet, moneyOperationsWalletDto.Password);

        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);

        if(user.Balance < moneyOperationsWalletDto.Amount)
        {
            throw new BadHttpRequestException("Your balance is too low to charge your wallet.");
        }

        user.Balance -= moneyOperationsWalletDto.Amount;
        wallet.Balance += (moneyOperationsWalletDto.Amount / (wallet.Currency.Value * (1.0m + (decimal)_userContextService.GetUserFee)));

        _dbContext
            .Users
            .Update(user);
        _dbContext
            .Wallets
            .Update(wallet);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task WithdrawWalletAsync(int id, MoneyOperationsWalletDto moneyOperationsWalletDto)
    {
        var wallet = await
            FirstWalletWithCurrencyAsync(w => w.Id == id);

        await VerifyWalletOwner(wallet);
        VerifyWalletPassword(wallet, moneyOperationsWalletDto.Password);

        wallet.Balance -= (moneyOperationsWalletDto.Amount / (wallet.Currency.Value * (1.0m - (decimal)_userContextService.GetUserFee)));
        if(wallet.Balance < 0)
        {
            throw new BadHttpRequestException("Your wallet balance is too low for withdraw.");
        }

        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);

        user.Balance += moneyOperationsWalletDto.Amount;

        _dbContext
            .Users
            .Update(user);
        _dbContext
            .Wallets
            .Update(wallet);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task SendCurrenciesToOtherWalletAsync(int id, SendMoneyOperationsWalletDto sendMoneyOperationsWalletDto)
    {
        var sourceWallet = await
            FirstWalletWithCurrencyAsync(w => w.Id == id);

        await VerifyWalletOwner(sourceWallet);
        VerifyWalletPassword(sourceWallet, sendMoneyOperationsWalletDto.Password);

        if(sourceWallet.WalletId == sendMoneyOperationsWalletDto.DestinationGuid)
        {
            throw new BadHttpRequestException("Choose different DestinationGuid");
        }

        sourceWallet.Balance -= (sendMoneyOperationsWalletDto.Amount / sourceWallet.Currency.Value);
        if (sourceWallet.Balance < 0)
        {
            throw new BadHttpRequestException("Your wallet balance is too low for withdraw.");
        }

        var destinationWallet = await 
            FirstWalletWithCurrencyAsync(w => w.WalletId == sendMoneyOperationsWalletDto.DestinationGuid,
            "Wallet with provided DestinationGuid does not exist.");

        destinationWallet.Balance += (sendMoneyOperationsWalletDto.Amount / destinationWallet.Currency.Value);

        _dbContext
            .Wallets
            .UpdateRange(sourceWallet, destinationWallet);
        await _dbContext
            .SaveChangesAsync();
    }

    private async Task<Wallet> FirstWalletWithCurrencyAsync(Expression<Func<Wallet, bool>> predicate, string? notFoundMessage = null)
    {
        var wallet = await _dbContext
            .Wallets
            .Include(w => w.Currency)
            .FirstOrDefaultAsync(predicate);

        if(!string.IsNullOrEmpty(notFoundMessage)
            && wallet is null)
        {
            throw new BadHttpRequestException(notFoundMessage);
        }

        return wallet;
    }

    private void VerifyWalletPassword(Wallet wallet, string providedPassword)
    {
        var passwordCorrect = _passwordHasher.VerifyHashedPassword(wallet, wallet.HashedPassword, providedPassword);

        if (passwordCorrect == PasswordVerificationResult.Failed)
        {
            throw new BadHttpRequestException("Incorrect walletName or password.");
        }
    }

    private async Task VerifyWalletOwner(Wallet wallet)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, wallet,
            new EntityOwnerRequirement());

        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException("You can't access to this wallet.");
        }
    }

    private async Task<Dictionary<string, Currency>> GetCurrenciesAsync()
    {
        var currencies = await _dbContext
            .Currencies
            .ToDictionaryAsync(c => c.Name.ToLower());

        return currencies;
    }
}
