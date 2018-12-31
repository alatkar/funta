﻿using core.repository.azureCosmos;
using MainService.Models.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Models
{
    [JsonObject]
    public class User : DocumentBase
    {
        // User readable name. We expect this to be used.
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

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
