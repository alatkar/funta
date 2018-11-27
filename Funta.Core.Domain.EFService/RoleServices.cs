using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.Domain.Entity.Enums;
using Funta.Core.DTO.Roles;
using Funta.Core.Infrastructures.DataAccess;
using Funta.Core.Infrastructures.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.EFService
{
    public class RoleServices : BaseRepository<Roles, RolesDto, RolesEnum>, IRoleServices
    {
        private readonly IUserRoleServices _roleServices;

        public RoleServices(IUnitOfWorks uow,
            IUserRoleServices roleServices) : base(uow)
        {
            _roleServices = roleServices;
        }

        public async Task<List<Roles>> FindUserRolesAsync(Guid userId) =>
           await _dbSet.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.User).Where(x => x.UserRoles.Any(z => z.User.Id == userId)).ToListAsync();

        public async Task<List<Users>> FindUsersInRoleAsync(string roleName)
        {
            Roles roles = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Name.Equals(roleName));
            return await _roleServices.GetDbSet().AsNoTracking().Include(x => x.User).Where(x => x.RoleKey == roles.Id).Select(x => x.User).ToListAsync();
        }

        public async Task<bool> IsUserInRole(Guid userId, string roleName)
        {
            Roles roles = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Name.Equals(roleName));
            return await _roleServices.GetDbSet().AnyAsync(x => x.RoleKey == roles.Id && x.UserKey == userId);
        }
    }
}
