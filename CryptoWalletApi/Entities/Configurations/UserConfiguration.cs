using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletApi.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Nickname)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.FullName)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .IsRequired();

        builder.HasOne(u => u.Address)
            .WithOne(u => u.User)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Wallets)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(u => u.Balance)
            .HasDefaultValue(0)
            .HasColumnType("decimal(38,6)");
    }
}
