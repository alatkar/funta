// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class Address
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }
        [JsonProperty("line2")]
        public string Line2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }
    }
}
