using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace core.repository.azureCosmos
{
    public abstract class DocumentBase : Microsoft.Azure.Documents.Document
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}