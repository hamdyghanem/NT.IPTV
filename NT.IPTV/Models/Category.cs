using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models
{
    public class StreamCategory
    {
        [JsonProperty("category_id")]
        public string ID { get; set; } = string.Empty;

        [JsonProperty("category_name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        public override string? ToString()
        {
            return Name;
        }

    }
}
