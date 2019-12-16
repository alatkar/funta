// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PartyFindsApi.Models;

namespace PartyFindsApi.core
{
    public class CosmosDbService : ICosmosDbService
    {
        private Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Listings item)
        {
            await this._container.CreateItemAsync<Listings>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Listings>(id, new PartitionKey(id));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Container> GetContainer(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Listings> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Listings> response = await this._container.ReadItemAsync<Listings>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Listings>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Listings>(new QueryDefinition(queryString));
            List<Listings> results = new List<Listings>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Listings item)
        {
            await this._container.UpsertItemAsync<Listings>(item, new PartitionKey(id));
        }
    }
}
