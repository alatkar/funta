// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartyFindsApi.Models;
using Microsoft.Azure.Documents.Client;

namespace PartyFindsApi.core
{
    public interface IRepository : IDisposable
    {
        Task<T> CreateAsync<T>(T doc, RequestOptions options);

        Task<T> UpdateAsync<T>(T doc, RequestOptions options) where T : DocumentBase;

        Task DeleteAsync(string docId, RequestOptions options);

        Task<T> GetAsync<T>(string id, RequestOptions options);

        Task<IList<T>> QueryAsync<T>(string filter, FeedOptions options);

        Task<T> CreateIfNotExists<T>(T doc, FeedOptions options) where T : DocumentBase;
    }
}
