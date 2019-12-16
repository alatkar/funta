// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class Listings : DocumentBase
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "listingType")]        
        public string ListingType { get; set; }

        [JsonProperty(PropertyName = "listingSubType")]        
        public string ListingSubType { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "radius")]
        public int radius { get; set; }

        [JsonProperty("imageUrls")]
        public IList<string> ImageUrl { get; set; }  //TODO Create List
    }
}
