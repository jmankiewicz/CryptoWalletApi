namespace CryptoWalletApi.Models;

public class CurrencyDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal Value { get; set; }
    public string IconPath { get; set; }
    public decimal Rate { get; set; }
}
