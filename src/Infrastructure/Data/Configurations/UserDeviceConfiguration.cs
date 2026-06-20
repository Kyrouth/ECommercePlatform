using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class UserDeviceConfiguration
    : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable("UserDevices");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.ClientId)
            .IsRequired();


        builder.HasIndex(x => x.ClientId)
            .IsUnique();


        builder.Property(x => x.UserAgent)
            .HasMaxLength(500);


        builder.Property(x => x.IpAddress)
            .HasMaxLength(100);


        builder.Property(x => x.UserId)
            .IsRequired(false);


        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}