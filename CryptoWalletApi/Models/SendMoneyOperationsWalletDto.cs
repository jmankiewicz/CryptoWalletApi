
namespace CryptoWalletApi.Models;

public class SendMoneyOperationsWalletDto
{
    public string Password { get; set; }
    public decimal Amount { get; set; }
    public Guid DestinationGuid { get; set; }
}
