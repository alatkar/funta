/*  
 *  PartyFinds LLC 
 *  All rights reservced
*/
// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using PartyFindsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyFindsApi.core
{
    public interface ICosmosDbService: IDisposable
    {
        Task<IEnumerable<Listings>> GetItemsAsync(string query);
        Task<Listings> GetItemAsync(string id);
        Task AddItemAsync(Listings item);
        Task UpdateItemAsync(string id, Listings item);
        Task DeleteItemAsync(string id);
        Task<Container> GetContainer(string id);
    }
}
