using System.Runtime.Serialization;

namespace FeedService.Models
{
    public class TipFeed: IFeed
    {
        [DataMember(Name = "location")]
        
        public string Topic { get; set; }

        public TipFeed(string topic)
        {
            this.Topic = topic;

        }
    }
}