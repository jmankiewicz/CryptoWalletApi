using CryptoWalletApi.Entities;

namespace CryptoWalletApi;

public class Seeder
{
    private readonly CryptoWalletDbContext _dbContext;

    public Seeder(CryptoWalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedRoles()
    {
        var roles = new List<Role>
        {
            new Role
            {
                Name = "User",
                Fee = 0.012m,
            },
            new Role
            {
                Name = "Premium",
                Fee = 0.007m,
            },
            new Role
            {
                Name = "Admin",
                Fee = 0.0m,
            },
        };

        await _dbContext
            .Roles
            .AddRangeAsync(roles);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task SeedCurrencies()
    {
        var currencies = new List<Currency>
        {
            new Currency
            {
                Name = "Ethereum",
                Code = "ETH",
                Value = 2421.34m
            },
            new Currency
            {
                Name = "Tether",
                Code = "USDT",
                Value = 1.00m
            },
            new Currency
            {
                Name = "BNB",
                Code = "BNB",
                Value = 552.11m
            },
            new Currency
            {
                Name = "Solana",
                Code = "SOL",
                Value = 137.06m
            },
            new Currency
            {
                Name = "Avalanche",
                Code = "AVAX",
                Value = 24.81m
            },
            new Currency
            {
                Name = "Bitcoin Cash",
                Code = "BCH",
                Value = 330.58m
            },
        };

        await _dbContext
            .Currencies
            .AddRangeAsync(currencies);
        await _dbContext
            .SaveChangesAsync();
    }
}
