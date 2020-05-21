// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class Listing : DocumentBase
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        // Free form text for pitch (specialities, tips and service details)
        [JsonProperty(PropertyName = "businessPitch")]
        public string BusinessPitch { get; set; }

        [JsonProperty(PropertyName = "businessStartDate")]
        public DateTimeOffset BusinessStartDate { get; set; }

        // TODO: Can this be List??
        [JsonProperty(PropertyName = "customerGroup")]
        public string CustomerGroup { get; set; }

        // TODO: Can this be Enum?? Outside world should see it as string
        // Type of service (Entertainment, Venue etc)
        [JsonProperty(PropertyName = "listingType")]
        public string ListingType { get; set; }

        // TODO: Can this be Enum?? Outside world should see it as string
        // Subtypes for each service (For Entrtainment: Musician, Magician etc)
        [JsonProperty(PropertyName = "listingSubType")]
        public string ListingSubType { get; set; }

        // TODO: Can this be Enum?? Outside world should see it as string
        // Venue Type (Indoor, Outdoor, both)
        [JsonProperty(PropertyName = "venueType")]
        public string VenueType { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "serviceRadius")]
        public int ServiceRadius { get; set; }

        // User owning this listing
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        // Upalod images
        [JsonProperty("imageUrls")] 
        public IList<string> ImageUrls { get; } = new List<string>();

        // Upalod video/audio
        [JsonProperty("mediaUrls")]
        public IList<string> MediaUrls { get; } = new List<string>();
    }
}
