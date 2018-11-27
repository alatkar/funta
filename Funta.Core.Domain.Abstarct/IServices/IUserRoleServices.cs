using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Domain.Abstarct.IServices
{
    public interface IUserRoleServices : IBaseRepository<UserRole, UserRole, int>
    {
    }
}
