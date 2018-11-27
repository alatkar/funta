using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.DTO.User;
using Funta.Core.Helper;
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
    public class RegisterServices : BaseRepository<Users, RegisterUserDto, Guid>, IRegisterServices
    {
        private readonly IUnitOfWorks _uow;

        public RegisterServices(IUnitOfWorks uow) : base(uow)
        {
            _uow = uow;
        }
        public async Task UpdateUserLastActivityDateAsync(Guid userId)
        {
            Users user = await _dbSet.FindAsync(userId);
            user.UpdateDate = DateTime.Now;
            _uow.Update(user);
        }

        public async Task<(StatusUserEnum, ProfileDto)> Verify(LoginUserDto model)
        {
            Users user = await _dbSet.AsNoTracking().Where(x => x.Mobile.ToLowerInvariant() == model.Mobile.ToLowerInvariant()).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.IsActive)
                {
                    ProfileDto profile = new ProfileDto
                    {
                        BirthDay = user.BirthDay,
                        City = user.City,
                        Family = user.Family,
                        LastActivity = user.UpdateDate,
                        Mobile = user.Mobile,
                        Name = user.Name,
                        UserId = user.Id,
                        SerialNumber = user.SerialNumber
                    };
                    string pass = Hash.Create(model.Password, user.SaltForHashing);
                    if (pass == user.Password)
                        return (StatusUserEnum.LoggeIn, profile);
                }
                else
                    return (StatusUserEnum.NotActive, null);
            }
            return (StatusUserEnum.UserNameNotExist, null);
        }
    }
}
