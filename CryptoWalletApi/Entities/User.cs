namespace CryptoWalletApi.Entities;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    public virtual Role Role { get; set; }
    public int RoleId { get; set; } = 1;

    public Address? Address { get; set; }

    public List<Wallet> Wallets { get; set; } = new List<Wallet>();
}