using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Models.Profile
{
    [JsonObject]
    public abstract class Profile
    {
        // Internally generated ID for the Profile
        // TODO: Implement ID generation
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        // User Readable Name. We expect this to be used.
        [JsonProperty("profileName")]
        public string ProfileName { get; set; }

        [JsonProperty("profileType")]
        public ProfileType ProfileType { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
