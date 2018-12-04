using Funta.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.Abstarct.Repositories.Base
{
    public interface IBaseRepositoryCosmos<TEntity, Type> : IDisposable
                            where TEntity : BaseEntity<Type>, IAuditable, new()
    {
        Task<bool> AnyAsync(Type id);
        Task<TEntity> GetAsync(Type id);
        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> GetAllAsync(IList<Type> ids);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(IEnumerable<TEntity> entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(string id);
        Task<IList<TEntity>> QueryAsync(string filter);
    }
}
