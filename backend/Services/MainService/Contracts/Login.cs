using Newtonsoft.Json;

namespace MainService.Contracts
{
    [JsonObject]
    public class Login
    {
        [JsonProperty(PropertyName = "userName", Required = Required.Always)]
        public string UserName { get; set; }

        //TODO: Should email be used to login (for now, no)
        //[JsonProperty(PropertyName = "email")]
        //public virtual string Email { get; set; }

        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        public virtual string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "remember")]
        public virtual bool Remember { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}