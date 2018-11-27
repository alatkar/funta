using Funta.Core.Domain.Entity.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Infrastructures.DataAccess.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasIndex(z => z.Mobile).IsUnique(true);
            builder.HasIndex(z => z.Email).IsUnique(true);
            builder.Property(z => z.IsRemoved).HasDefaultValue(false);
            builder.Property(z => z.RegDate).HasDefaultValue(new DateTime(2018, 11, 25, 00, 00, 00));
            builder.HasOne(z => z.UserToken).WithOne(z => z.User).HasForeignKey<UserToken>(z => z.UserKey).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.UserName).HasMaxLength(50);
            builder.HasData(new Users {
                BirthDay = new DateTime(1986,03,02),
                City = "Tehran",
                DisplayName ="Milad",
                Email = "milad.jafari@live.com",
                Family = "Jafari",
                Id = new Guid("4A7E613D-D2EA-457F-8B13-21F927258192"),
                IsActive = true,
                IsRemoved = false,
                LastLoggedIn = null,
                Mobile = "+989022020735",
                Name = "Milad",
                Password = "",//123456
               RegDate = new DateTime(2018, 11, 25, 00, 00, 00),
               SaltForHashing = "Microsoft",
               SerialNumber = "4A7E613D-D2EA-457F-8B13-21F927258192",
               State = "Tehran",
               UpdateDate = null,
               UserName = "miladj3"
            });
        }
    }
}
