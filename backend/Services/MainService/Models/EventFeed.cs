using System.Runtime.Serialization;

namespace MainService.Models
{
    public class EventFeed : IFeed
    {
        public EventFeed(string location)
        {
            this.Type = FeedTypes.EVENT;
            this.Location = location;
        }

        [DataMember(Name = "location")]
        public string Location { get; set; }
    }
}