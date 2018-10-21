using System;
using Newtonsoft.Json;

namespace Entities
{
    public class Category
    {
        [JsonProperty("cid")]
        public int ID{ get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
