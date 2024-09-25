using AutoMapper;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Models;

namespace CryptoWalletApi;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<AddAddressDto, Address>();
        CreateMap<CurrencyDto, Currency>();
        CreateMap<Currency, CurrencyDto>();
        CreateMap<CreateWalletDto, Wallet>();
        CreateMap<Wallet, WalletDto>()
            .ForMember(w => w.CurrencyName, m => m.MapFrom(w => w.Currency.Name))
            .ForMember(w => w.OwnerNickname, m => m.MapFrom(w => w.User.Nickname));
    }
}
