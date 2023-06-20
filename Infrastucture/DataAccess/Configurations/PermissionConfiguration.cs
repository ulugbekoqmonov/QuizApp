using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p=>p.Id);

        builder.Property(p=>p.PermissionName)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(p=>p.Id)
            .IsUnique(true);                          
    }
}
