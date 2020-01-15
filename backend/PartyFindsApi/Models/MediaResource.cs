﻿// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyFindsApi.Models
{
    [JsonObject]
    public class MediaResource
    {
        [JsonProperty("file")]
        public IFormFile File { get; set; }

        [JsonProperty("lastName")]
        public string contentType { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

    }
}
