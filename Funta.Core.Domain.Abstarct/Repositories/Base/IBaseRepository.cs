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
    public interface IBaseRepository<T, Type> where T : class
    {
        Task<T> InsertAsync(T entity);
        Task<SearchResult<T, BaseSearchParameter>> GetListAsync(BaseSearchParameter searchParameters);
        Task<SearchResult<T>> GetListAsync();
        Task<T> FindAsync(Type id);
        T Find(Type id);
        Task<bool> IsExistAsync(Type id);
        Task<Type> DeleteAsync(Type id);
        Task RemoveRangeAsync(List<T> items);
        Task InsertRangeAsync(List<T> items);
        Task<Type> UpdateAsync(T entity);
        Task<Type> UpdateRangeAsync(List<T> items);
        IQueryable<T> GetDbSet(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        DbSet<T> GetDbSet();
    }
}
