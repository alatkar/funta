using System;
using Newtonsoft.Json;

public class Feed : FeedService.Models.IFeed
{
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}