using Funta.Core.DTO.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Domain.Abstarct.Repositories.Base
{
    public interface IBaseRepositoryDapper<T, TDto, Type> where T : class
    {
        Task<SearchResult<J>> GetListAsync<J>(string Sql, object param) where J : class;
    }
}
