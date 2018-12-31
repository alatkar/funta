using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace core.repository.azureCosmos
{
    public abstract class DocumentBase //: Microsoft.Azure.Documents.Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; }

        [JsonProperty("isActive")]
        public string IsActive { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}