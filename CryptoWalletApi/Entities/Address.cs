namespace CryptoWalletApi.Entities;

public class Address
{
    public int Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public User User { get; set; }
    public int UserId { get; set; }
}