namespace CryptoWalletApi.Entities;

public class Currency
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal? Rate { get; set; }
    public string? IconPath { get; set; }
}
