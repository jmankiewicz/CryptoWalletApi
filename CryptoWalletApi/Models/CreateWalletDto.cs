using CryptoWalletApi.Entities;

namespace CryptoWalletApi.Models;

public class CreateWalletDto
{
    public string WalletName { get; set; }
    public string CurrencyName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}