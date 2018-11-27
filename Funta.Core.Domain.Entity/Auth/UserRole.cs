using Funta.Core.Domain.Entity.Base;
using Funta.Core.Domain.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funta.Core.Domain.Entity.Auth
{
    [Table(name: "UserRole", Schema = "dbo")]
    public class UserRole : BaseEntity<int>
    {
        [ForeignKey(nameof(UserKey))]
        public virtual Users User { get; set; }
        public Guid UserKey { get; set; }

        [ForeignKey(nameof(RoleKey))]
        public virtual Roles Role { get; set; }
        public RolesEnum RoleKey { get; set; }
    }
}
