using Funta.Core.Domain.Entity.Base;
using Funta.Core.Domain.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funta.Core.Domain.Entity.Auth
{
    [Table("Role", Schema = "dbo")]
    public class Roles : BaseEntity<RolesEnum>
    {
        public Roles()
        {
            UserRoles = new HashSet<UserRole>();
        }
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
