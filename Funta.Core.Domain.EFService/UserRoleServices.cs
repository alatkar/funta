using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.Infrastructures.DataAccess;
using Funta.Core.Infrastructures.DataAccess.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Domain.EFService
{
    public class UserRoleServices : BaseRepository<UserRole, UserRole, int>, IUserRoleServices
    {
        public UserRoleServices(IUnitOfWorks uow) : base(uow)
        {

        }
    }
}
