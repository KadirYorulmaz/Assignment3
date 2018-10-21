using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Response
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
