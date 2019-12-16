// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Newtonsoft.Json;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class Account : DocumentBase
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public virtual string Email { get; set; }

        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        public virtual string PasswordHash { get; set; }
    }
}
