using Newtonsoft.Json;

namespace MainService.Contracts
{
    [JsonObject]
    public class Register
    {
        [JsonProperty(PropertyName = "userName", Required = Required.Always)]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public virtual string Email { get; set; }

        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        public virtual string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "phoneNumber")]
        public virtual string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "zipCode", Required = Required.Always)]
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
