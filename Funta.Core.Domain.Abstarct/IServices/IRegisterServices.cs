using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.Abstarct.IServices
{
    public interface IRegisterServices : IBaseRepository<Users, RegisterUserDto, Guid>
    {
        Task<(StatusUserEnum, ProfileDto)> Verify(LoginUserDto model);
        Task UpdateUserLastActivityDateAsync(Guid userId);
    }
}
