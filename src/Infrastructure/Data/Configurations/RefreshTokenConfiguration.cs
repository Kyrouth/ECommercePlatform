using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HashToken)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.RevokedAt)
            .IsRequired(false);

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();


        builder.HasIndex(x => new
        {
            x.UserId,
            x.DeviceId
        })
        .IsUnique();


        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserDevice>()
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}