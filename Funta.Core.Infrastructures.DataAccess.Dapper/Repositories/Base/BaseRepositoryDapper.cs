using Dapper;
using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Base;
using Funta.Core.DTO.General;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Infrastructures.DataAccess.Dapper.Repositories.Base
{
    public abstract class BaseRepositoryDapper<T, TDto, Type> : IBaseRepositoryDapper<T, TDto, Type> where T : BaseEntity<Type>, new()
    {
        private readonly IConfiguration _configuration;

        public BaseRepositoryDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public virtual async Task<SearchResult<J>> GetListAsync<J>(string Sql, object param) where J : class
        {
            try
            {
                SearchResult<J> entity = new SearchResult<J>();
                IEnumerable<J> result;
                CreateConnection().Open();
                using (var connection = CreateConnection())
                {
                    result = await connection.QueryAsync<TDto>(Sql, param) as IEnumerable<J>;
                    CreateConnection().Close();
                }
                entity.Result = (List<J>)result;
                return entity;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("Connection");
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
