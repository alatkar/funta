using Funta.Core.Domain.Entity.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Infrastructures.DataAccess.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.HasData(new List<Roles> {
                new Roles
                {
                    Id = new Guid("7CEE6CCA-A8A2-42EC-AA2B-EE10AA9230E3"),
                    IsRemoved = false,
                    Name = "Admin",
                    RegDate = new DateTime(2018, 11, 25)
                },
                 new Roles
                {
                    Id = new Guid("D909755A-0068-4157-BA16-277528C44354"),
                    IsRemoved = false,
                    Name = "User",
                    RegDate = new DateTime(2018, 11, 25)
                }
            });
            builder.Ignore(x => x.RegDate);
            builder.Ignore(x => x.UpdateDate);
        }
    }
}
