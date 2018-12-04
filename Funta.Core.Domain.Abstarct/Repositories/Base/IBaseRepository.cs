using Funta.Core.Domain.Entity.Base;
using Funta.Core.DTO.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.Abstarct.Repositories.Base
{
    public interface IBaseRepository<TEntity, TDto, Type>
                            where TEntity : class, IAuditable
                            where TDto : class
    {
        Task<TDto> InsertAsync(TDto entity);
        Task<SearchResult<TEntity, BaseSearchParameter>> GetListAsync(BaseSearchParameter searchParameters);
        Task<SearchResult<TEntity>> GetListAsync();
        Task<TDto> FindAsync(Type id);
        TEntity Find(Type id);
        Task<bool> IsExistAsync(Type id);
        Task<Type> DeleteAsync(Type id);
        Task RemoveRangeAsync(List<TEntity> items);
        Task InsertRangeAsync(List<TEntity> items);
        Task<Type> UpdateAsync(TEntity entity);
        Task<Type> UpdateRangeAsync(List<TEntity> items);
        IQueryable<TEntity> GetDbSet(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAll();
        DbSet<TEntity> GetDbSet();
        string nameCache { get; }
        void ClearCache();
    }
}
