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
                    Id =  Domain.Entity.Enums.RolesEnum.Admin,
                    IsRemoved = false,
                    Name = "Admin",
                    RegDate = new DateTime(2018, 11, 25)
                },
                 new Roles
                {
                    Id =  Domain.Entity.Enums.RolesEnum.User,
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
