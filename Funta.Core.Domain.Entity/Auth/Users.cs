using Funta.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funta.Core.Domain.Entity.Auth
{
    [Table(name: "Users", Schema = "dbo")]
    public class Users : BaseEntity<Guid>
    {
        [DataType("int")]
        public string Mobile { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Family { get; set; }

        [DataType(DataType.Text)]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 0)]
        public string State { get; set; }

        [DataType(DataType.Password)]
        [MaxLength]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        public string SerialNumber { get; set; }
        public string SaltForHashing { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual UserToken UserToken { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastLoggedIn { get; set; }
    }
}
