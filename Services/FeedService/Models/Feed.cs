using System;
using Newtonsoft.Json;

public class Feed
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Data { get; set; }
    public string ImageUrl { get; set; }
    public FeedTypes Type { get; set; }
    public Feed[] Followups { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateLastUpdated { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}