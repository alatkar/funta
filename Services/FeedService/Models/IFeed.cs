
using System;
using System.Runtime.Serialization;
using core.repository.azureCosmos;
using Newtonsoft.Json;

namespace FeedService.Models
{
    public abstract class IFeed : DocumentBase
    {
        public IFeed()
        {
        }


        [DataMember(Name =  "userName")]
        public string UserName { get; set; }
        [DataMember(Name =  "data")]
        public string Data { get; set; }
        [DataMember(Name =  "imageUrl")]
        public string ImageUrl { get; set; }
        [DataMember(Name =  "type")]
        public FeedTypes Type { get; set; }
        [DataMember(Name =  "followups")]
        public Feed[] Followups { get; set; }
        [DataMember(Name =  "dateCreated")]
        public DateTimeOffset DateCreated { get; set; }
        [DataMember(Name =  "dateLastUpdated")]
        public DateTimeOffset DateLastUpdated { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
}
    }