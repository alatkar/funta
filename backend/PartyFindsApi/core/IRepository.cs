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
        Task<T> CreateAsync<T>(T doc, FeedOptions options);

        Task<T> UpdateAsync<T>(T doc, FeedOptions options) where T : DocumentBase;

        Task DeleteAsync(string docId, FeedOptions options);

        Task<T> GetAsync<T>(string id, FeedOptions options);

        Task<IList<T>> QueryAsync<T>(string filter, FeedOptions options);

        Task<T> CreateIfNotExists<T>(T doc, FeedOptions options) where T : DocumentBase;
    }
}
