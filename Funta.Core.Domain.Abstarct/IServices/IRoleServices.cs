using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.Domain.Entity.Enums;
using Funta.Core.DTO.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.Abstarct.IServices
{
    public interface IRoleServices : IBaseRepository<Roles, RolesDto, RolesEnum>
    {
        Task<List<Roles>> FindUserRolesAsync(Guid userId);
        Task<bool> IsUserInRole(Guid userId, string roleName);
        Task<List<Users>> FindUsersInRoleAsync(string roleName);
    }
}
