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
        Task<IEnumerable<Listing>> GetItemsAsync(string query);
        Task<Listing> GetItemAsync(string id);
        Task AddItemAsync(Listing item);
        Task UpdateItemAsync(string id, Listing item);
        Task DeleteItemAsync(string id);
        Task<Container> GetContainer(string id);
    }
}
