using Funta.Core.Domain.Entity.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Infrastructures.DataAccess.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.RoleKey, x.UserKey });
            builder.Ignore(x => x.Id);
            builder.HasOne(z => z.Role).WithMany(z => z.UserRoles).HasForeignKey(z => z.RoleKey).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(z => z.User).WithMany(z => z.UserRoles).HasForeignKey(z => z.UserKey).OnDelete(DeleteBehavior.Cascade);
            builder.HasData(new List<UserRole> {
                new UserRole{
                    IsRemoved = false,
                    RegDate = new DateTime(2018,11,25),
                    RoleKey = new Guid("7CEE6CCA-A8A2-42EC-AA2B-EE10AA9230E3"),
                    UserKey = new Guid("4A7E613D-D2EA-457F-8B13-21F927258192")
                },
                new UserRole{
                    IsRemoved = false,
                    RegDate = new DateTime(2018,11,25),
                    RoleKey = new Guid("D909755A-0068-4157-BA16-277528C44354"),
                    UserKey = new Guid("4A7E613D-D2EA-457F-8B13-21F927258192")
                }
            });
        }
    }
}
