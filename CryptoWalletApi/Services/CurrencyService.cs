using AutoMapper;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Services;

public class CurrencyService
{
    private readonly IMapper _mapper;
    private readonly CryptoWalletDbContext _dbContext;

    public CurrencyService(CryptoWalletDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task AddCurrencyAsync(CurrencyDto currencyDto)
    {
        var currency = _mapper.Map<Currency>(currencyDto);

        await _dbContext
            .Currencies
            .AddAsync(currency);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task UpdateCurrencyAsync(CurrencyDto currencyDto, int id)
    {
        var currency = _mapper.Map<Currency>(currencyDto);
        currency.Id = id;

        _dbContext
            .Currencies
            .Update(currency);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<List<CurrencyDto>> GetAllCurrenciesAsync(PaginationSettings paginationSettings)
    {
        var currencies = await _dbContext
            .Currencies
            .Skip((paginationSettings.PageNumber - 1) * paginationSettings.PageSize)
            .Take(paginationSettings.PageSize)
            .ToListAsync();

        var currenciesDto = _mapper.Map<List<CurrencyDto>>(currencies);

        return currenciesDto;
    }

    public async Task<CurrencyDto> GetCurrencyByIdAsync(int id)
    {
        var currency = await GetCurrencyById(id);
        var currencyDto = _mapper.Map<CurrencyDto>(currency);

        return currencyDto;
    }

    public async Task DeleteCurrencyAsync(int id)
    {
        var currency = await GetCurrencyById(id);

        _dbContext
            .Currencies
            .Remove(currency);
        await _dbContext
            .SaveChangesAsync();
    }

    private async Task<Currency> GetCurrencyById(int id)
    {
        var currency = await _dbContext
            .Currencies
            .FirstOrDefaultAsync(c => c.Id == id);

        if (currency is null)
        {
            throw new ArgumentException($"Currency with id: {id} not found.");
        }

        return currency;
    }
}
