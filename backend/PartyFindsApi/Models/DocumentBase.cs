// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public abstract class DocumentBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; }

        [JsonProperty("isActive")]
        public string IsActive { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
