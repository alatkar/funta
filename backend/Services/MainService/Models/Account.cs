using core.repository.azureCosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Models
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
