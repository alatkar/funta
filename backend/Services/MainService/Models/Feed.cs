using System;
using System.Runtime.Serialization;
using core.repository.azureCosmos;
using Newtonsoft.Json;

namespace MainService.Models
{
    public class Feed : DocumentBase //: MainService.Models.IFeed
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("type")]
        public FeedTypes Type { get; set; }
        [JsonProperty("followups")]
        public Feed[] Followups { get; set; }
        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }
        [JsonProperty("dateLastUpdated")]
        public DateTimeOffset DateLastUpdated { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}