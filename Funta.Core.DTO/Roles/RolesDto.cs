using Funta.Core.DTO.UserRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.DTO.Roles
{
    public class RolesDto
    {
        public string Name { get; set; }
        public virtual ICollection<UserRoleDto> UserRoles { get; set; }
    }
}
