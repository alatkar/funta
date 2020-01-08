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
    public class Notification : DocumentBase
    {
        [JsonProperty("userId", Required = Required.Always)]
        public string UserId { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

        [JsonProperty(PropertyName = "actionUri")]
        public string ActionUri { get; set; }

        [JsonProperty("accessFailedCount")]
        public int AccessFailedCount { get; set; }

        [JsonProperty("isRead")]
        public bool IsRead { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public DateTimeOffset CreationDate { get; set; }
        
        [JsonProperty(PropertyName = "ackDate")]
        public DateTimeOffset AckDate { get; set; }
    }
}
