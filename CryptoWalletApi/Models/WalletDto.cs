namespace CryptoWalletApi.Models;

public class WalletDto
{
    public string WalletName { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyName { get; set; }
    public string OwnerNickname { get; set; }
}
