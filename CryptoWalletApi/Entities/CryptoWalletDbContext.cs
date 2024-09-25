using Microsoft.EntityFrameworkCore;

namespace CryptoWalletApi.Entities;

public class CryptoWalletDbContext : DbContext
{
    public CryptoWalletDbContext(DbContextOptions<CryptoWalletDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
