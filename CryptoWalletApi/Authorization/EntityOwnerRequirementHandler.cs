using CryptoWalletApi.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CryptoWalletApi.Authorization;

public class EntityOwnerRequirementHandler : AuthorizationHandler<EntityOwnerRequirement, Wallet>
{
    private readonly CryptoWalletDbContext _dbContext;

    public EntityOwnerRequirementHandler(CryptoWalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EntityOwnerRequirement requirement, Wallet wallet)
    {
        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        if(wallet.UserId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
