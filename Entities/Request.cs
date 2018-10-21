using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("date")]
        public long Date { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }

}
