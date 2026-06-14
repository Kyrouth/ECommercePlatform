using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class PhoneVerificationSessionConfiguration
    : IEntityTypeConfiguration<PhoneVerificationSession>
{
    public void Configure(
        EntityTypeBuilder<PhoneVerificationSession> builder)
    {
        builder.ToTable("PhoneVerificationSessions");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.OtpHash)
            .IsRequired()
            .HasMaxLength(500);


        builder.Property(x => x.DeviceId)
            .IsRequired();


        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();


        builder.Property(x => x.ExpiresAt)
            .IsRequired();


        builder.Property(x => x.OtpExpiresAt)
            .IsRequired();


        builder.Property(x => x.VerifiedAt)
            .IsRequired(false);



        builder.HasIndex(x => x.DeviceId);



        builder.HasIndex(x => new
        {
            x.DeviceId,
            x.Status
        });

        builder.HasOne<UserDevice>()
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(11)
                .IsRequired();
        });
    }
}