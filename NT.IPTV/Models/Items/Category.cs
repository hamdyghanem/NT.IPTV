using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NT.IPTV.Models.Items
{
    public class StreamCategory
    {
        [JsonProperty("category_id")]
        public string ID { get; set; } = string.Empty;

        [JsonProperty("category_name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("parent_id")]
        public string ParentId { get; set; } = string.Empty;
        public bool Favorite { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
