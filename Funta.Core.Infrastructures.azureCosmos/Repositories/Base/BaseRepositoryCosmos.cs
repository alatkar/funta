using Funta.Core.Domain.Abstarct.Repositories.Base;
using Funta.Core.Domain.Entity.Base;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using static Funta.Core.Infrastructures.azureCosmos.Config;

namespace Funta.Core.Infrastructures.azureCosmos.Repositories.Base
{
    public class BaseRepositoryCosmos<TEntity,Type> : IBaseRepositoryCosmos<TEntity, Type>
                            where TEntity : BaseEntity<Type>, IAuditable, new()
    {
        private DocumentClient _client;
        public BaseRepositoryCosmos()
        {
            _client = new DocumentClient(new Uri(DocumentDbEndpointUrl), DocumentDbPrimaryKey);
            _client.CreateDatabaseIfNotExistsAsync(new Database { Id = Config.DatabaseName }).Wait();
        }

        public async Task<bool> AnyAsync(Type id)
        {
            return await _client.CreateDocumentQuery<TEntity>
                        (UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name))
                        .AnyAsync(x => x.Id.Equals(id));
        }

        public async Task DeleteAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, typeof(TEntity).Name, id.ToString()));
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await _client.CreateDocumentQuery<TEntity>
                       (UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name)).ToListAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync(IList<Type> ids)
        {
            return await _client.CreateDocumentQuery<TEntity>
                        (UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name))
                        .Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Type id)
        {
            return await _client.CreateDocumentQuery<TEntity>
                        (UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name))
                        .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = typeof(TEntity).Name });
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name), entity);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = typeof(TEntity).Name });

            foreach (var entity in entities)
                await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name), entity);
        }

        public async Task<IList<TEntity>> QueryAsync(string filter)
        {
            var query = _client.CreateDocumentQuery<TEntity>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, ""),
                    $"SELECT * FROM {""} {filter} ")
                    .AsEnumerable().ToList();
            return query;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TEntity).Name), entity);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        ~BaseRepositoryCosmos()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}