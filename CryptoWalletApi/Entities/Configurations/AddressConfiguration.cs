using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletApi.Entities.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.Country)
            .IsRequired();

        builder.Property(a => a.City)
            .IsRequired();

        builder.Property(a => a.Region)
            .IsRequired();

        builder.Property(a => a.Street)
            .IsRequired();

        builder.Property(a => a.PostalCode)
            .IsRequired();
    }
}
