// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class User : Account
    {
        [JsonProperty("address")]
        public virtual Address Address { get; set; }

        [JsonProperty("accessFailedCount")]
        public virtual int AccessFailedCount { get; set; }

        [JsonProperty("emailConfirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }

        [JsonProperty("phoneNumber")]
        public virtual string PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
