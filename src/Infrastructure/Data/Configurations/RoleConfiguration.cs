using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration
    : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Description)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.IsSystemRole)
            .IsRequired()
            .HasDefaultValue(false);


        builder.HasMany<User>()
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder
            .HasMany<Permission>()
            .WithMany()
            .UsingEntity("RolePermissions");

        // seed data

        builder.HasData(
            Role.Create(
                SystemRoles.OwnerId,
                "Owner",
                "The Owner role has the highest level of access. This is a system role and cannot be deleted or modified.",
                true
            ).Value,
            Role.Create(
                SystemRoles.CustomerId,
                "Customer",
                "The Customer role is the default role for website users. This is a system role and cannot be deleted or modified.",
                true
            ).Value
        );
    }
}