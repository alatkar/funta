using MainService.Models;
using MainService.Models.Profile;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Serialize
{
    public class JsonProfileConverter : JsonCreationConverter<Profile>  // TODO: NOT USED: DELTE if not needed
    {
        protected override Profile Create(Type objectType, JObject jsonObject)
        {
            var typeName = jsonObject["profileType"].ToString();
            switch (typeName)
            {
                case "DOG":
                    return new DogProfile();
                default: return null;
            }
        }
    }
}
