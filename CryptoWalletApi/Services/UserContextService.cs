using CryptoWalletApi.Entities;
using System.Security.Claims;

namespace CryptoWalletApi.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    int? GetUserId { get; }
    int? GetUserAddressId { get; }
    decimal? GetUserFee { get; }
}

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor, CryptoWalletDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;   
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    public int? GetUserId => 
        User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    public int? GetUserAddressId =>
        User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == "AddressId").Value);

    public decimal? GetUserFee =>
        User is null ? null : (decimal?)decimal.Parse(User.FindFirst(c => c.Type == "Fee").Value);
}
