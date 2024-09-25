namespace CryptoWalletApi;

public class AuthenticationSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int ExpireDays { get; set; }
}
