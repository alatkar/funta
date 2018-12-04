using AutoMapper;
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
using Z.EntityFramework.Plus;

namespace Funta.Core.Infrastructures.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<TEntity, TDto,Type> : IBaseRepository<TEntity, TDto, Type>
                            where  TDto: class
                            where TEntity :  BaseEntity<Type>, IAuditable, new()
    {
        protected DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorks _uow;

        public string nameCache => typeof(TEntity).FullName;

        protected BaseRepository(IUnitOfWorks uow)
        {
            _uow = uow;
            _dbSet = _uow.Set<TEntity>();
        }

        public virtual async Task<Type> DeleteAsync(Type id)
        {
            var entity = await _dbSet.FindAsync(id);
            entity.IsRemoved = true;
            _uow.SaveChanges();
            return entity.Id;
        }

        public virtual async Task RemoveRangeAsync(List<TEntity> items)
        {
            _dbSet.RemoveRange(items);
            await _uow.SaveChangesAsync();
        }

        public virtual async Task InsertRangeAsync(List<TEntity> items)
        {
            _dbSet.AddRange(items);
            await _uow.SaveChangesAsync();
        }

        public virtual async Task<SearchResult<TEntity, BaseSearchParameter>> GetListAsync(BaseSearchParameter searchParameters)
        {
            var result = new SearchResult<TEntity, BaseSearchParameter>
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

        public virtual async Task<TDto> InsertAsync(TDto entity)
        {
            var model = Mapper.Map<TEntity>(entity);
            _dbSet.Add(model);
            await _uow.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<Type> UpdateAsync(TEntity entity)
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

        public virtual async Task<TDto> FindAsync(Type id)
        {
            var model =   await _dbSet.FindAsync(id);
            return Mapper.Map<TDto>(model);
        }

        public virtual TEntity Find(Type id)
        {
            return _dbSet.Find(id);
        }
        

        public virtual IQueryable<TEntity> GetDbSet(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> localEntities = _dbSet.AsQueryable();
            if (expression != null)
            {
                localEntities = localEntities.Where(expression);
            }
            return localEntities;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual DbSet<TEntity> GetDbSet()
        {
            return _dbSet;
        }

        public virtual async Task<SearchResult<TEntity>> GetListAsync()
        {
            var result = new SearchResult<TEntity>();
            var query = _dbSet.AsNoTracking().OrderByDescending(c => c.Id).AsQueryable();

            result.Result = await query.ToListAsync();
            return result;
        }

        public virtual async Task<bool> IsExistAsync(Type id)
        {
            return await _dbSet.AnyAsync(x => x.Id.Equals(id));
        }

        public async Task<Type> UpdateRangeAsync(List<TEntity> items)
        {
            _dbSet.UpdateRange(items);
            await _uow.SaveChangesAsync();
            return default(Type);
        }

        public void ClearCache() =>
           QueryCacheManager.ExpireTag(this.nameCache);
    }
}
