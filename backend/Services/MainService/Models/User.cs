using core.repository.azureCosmos;
using MainService.Models.Profile;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MainService.Models
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

        // Profiles in this user
        [JsonProperty("dogProfiles")]
        public List<DogProfile> DogProfiles { get; } = new List<DogProfile>();

        [JsonProperty("productProfiles")]
        public List<ProductProfile> ProductProfiles { get; } = new List<ProductProfile>();

        [JsonProperty("serviceProfiles")]
        public List<ServiceProfile> ServiceProfiles { get; } = new List<ServiceProfile>();

        [JsonProperty("nonProfitProfiles")]
        public List<NonProfitProfile> NonProfitProfiles { get; } = new List<NonProfitProfile>();

        [JsonProperty("shelterProfiles")]
        public List<ShelterProfile> ShelterProfiles { get; } = new List<ShelterProfile>();
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
