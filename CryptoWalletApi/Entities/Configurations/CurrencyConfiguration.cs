using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletApi.Entities.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired();

        builder.Property(c => c.Code)
            .IsRequired();

        builder.Property(c => c.Value)
            .HasDefaultValue(0)
            .HasColumnType("decimal(38,6)");
    }
}
