// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class Message : DocumentBase
    {
        //toUserId
        [JsonProperty("toUserId", Required = Required.Always)]
        public string toUserId { get; set; }
        
        [JsonProperty("fromUserId", Required = Required.Always)]
        public string fromUserId { get; set; }

        [JsonProperty("listingId", Required = Required.Always)]
        public string ListingId { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("dateTime")]
        public DateTimeOffset dateTime { get; set; }

        [JsonProperty("responses")]
        public IList<Message> Responses { get; } = new List<Message>();

    }
}
