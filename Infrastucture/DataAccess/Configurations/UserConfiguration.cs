using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user=>user.Id);

        builder
            .Property(u=>u.FirstName).
            IsRequired()
            .HasMaxLength(50);

        builder
            .Property(u=>u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);
        builder
            .HasIndex(u=>u.UserName)
            .IsUnique(true);

        builder
            .Property(u=>u.Password)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(u => u.Email)
            .HasMaxLength(100);
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(14);
        builder.HasIndex(u=>u.PhoneNumber)
            .IsUnique();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
    }
}
