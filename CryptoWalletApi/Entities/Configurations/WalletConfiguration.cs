using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletApi.Entities.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(w => w.WalletName)
            .IsRequired();

        builder.Property(w => w.Balance)
            .HasDefaultValue(0)
            .HasColumnType("decimal(38,6)");
    }
}
