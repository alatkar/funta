
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FeedService.Models
{
    public abstract class IFeed
    {
        public IFeed()
        {
        }

        [DataMember(Name =  "id")]
        public Guid Id { get; set; }

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