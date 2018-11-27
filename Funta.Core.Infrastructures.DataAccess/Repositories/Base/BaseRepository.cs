using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Base;
using Funta.Core.DTO.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Infrastructures.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T, Type> : IBaseRepository<T, Type> where T : BaseEntity<Type>, new()
    {
        protected DbSet<T> _dbSet;
        private readonly IUnitOfWorks _uow;

        protected BaseRepository(IUnitOfWorks uow)
        {
            _uow = uow;
            _dbSet = _uow.Set<T>();
        }

        public virtual async Task<Type> DeleteAsync(Type id)
        {
            var entity = await _dbSet.FindAsync(id);
            entity.IsRemoved = true;
            _uow.SaveChanges();
            return entity.Id;
        }

        public virtual async Task RemoveRangeAsync(List<T> items)
        {
            _dbSet.RemoveRange(items);
            await _uow.SaveChangesAsync();
        }

        public virtual async Task InsertRangeAsync(List<T> items)
        {
            _dbSet.AddRange(items);
            await _uow.SaveChangesAsync();
        }

        public virtual async Task<SearchResult<T, BaseSearchParameter>> GetListAsync(BaseSearchParameter searchParameters)
        {
            var result = new SearchResult<T, BaseSearchParameter>
            {
                SearchParameter = searchParameters
            };
            var query = _dbSet.AsNoTracking().OrderByDescending(c => c.Id).AsQueryable();

            if (searchParameters.SearchParameter != default(DateTime))
            {
                query = query.Where(c => c.RegDate <= searchParameters.SearchParameter);
            }

            if (searchParameters.NeedTotalCount)
            {
                result.TotalCount = query.Count();
            }

            result.Result = await query.Take(searchParameters.PageSize).ToListAsync();
            return result;
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            _dbSet.Add(entity);
            await _uow.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<Type> UpdateAsync(T entity)
        {
            var model = await FindAsync(entity.Id);
            if (model == null)
            {
                return default(Type); //equal null
            }
            _uow.Entry(model).CurrentValues.SetValues(entity);
            await _uow.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task<T> FindAsync(Type id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual T Find(Type id)
        {
            return _dbSet.Find(id);
        }
        

        public virtual IQueryable<T> GetDbSet(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> localEntities = _dbSet.AsQueryable();
            if (expression != null)
            {
                localEntities = localEntities.Where(expression);
            }
            return localEntities;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual DbSet<T> GetDbSet()
        {
            return _dbSet;
        }

        public virtual async Task<SearchResult<T>> GetListAsync()
        {
            var result = new SearchResult<T>();
            var query = _dbSet.AsNoTracking().OrderByDescending(c => c.Id).AsQueryable();

            result.Result = await query.ToListAsync();
            return result;
        }

        public virtual async Task<bool> IsExistAsync(Type id)
        {
            return await _dbSet.AnyAsync(x => x.Id.Equals(id));
        }

        public async Task<Type> UpdateRangeAsync(List<T> items)
        {
            _dbSet.UpdateRange(items);
            await _uow.SaveChangesAsync();
            return default(Type);
        }
    }
}
