// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;
using System;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public abstract class DocumentBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "_rid")]
        public virtual string ResourceId { get; set; }

        //[Newtonsoft.Json.JsonConverter(typeof(Microsoft.Azure.Documents.UnixDateTimeConverter))]
        [JsonProperty("_ts")]
        public long _ts { get; }

        [JsonProperty("isActive")]
        public string IsActive { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
