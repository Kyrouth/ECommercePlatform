using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.OwnsOne(x => x.Username, username =>
        {
            username.Property(x => x.Value)
                .HasColumnName("Username")
                .HasMaxLength(Username.MaxLength)
                .IsRequired();

            username.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsOne(x => x.PhoneNumber, phoneNumber =>
        {
            phoneNumber.Property(x => x.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(PhoneNumber.CharacterLength)
                .IsRequired();

            phoneNumber.HasIndex(x => x.Value)
                .IsUnique();
        });
    }
}