using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Models.Profile
{
    [JsonObject]
    public class DogProfile : Profile
    {
        [JsonProperty("breed")]
        public string Breed { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("medicalRecords")]
        public string MedicalRecords { get; set; }
    }
}
