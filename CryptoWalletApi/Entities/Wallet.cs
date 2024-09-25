namespace CryptoWalletApi.Entities;

public class Wallet
{
    public int Id { get; set; }
    public string WalletName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string HashedPassword { get; set; } = string.Empty;
    public Guid WalletId { get; set; }

    public virtual Currency Currency { get; set; }
    public int CurrencyId { get; set; }

    public virtual User User { get; set; }
    public int UserId { get; set; }
}
