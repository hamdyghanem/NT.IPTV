using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NT.IPTV.Models.Channel
{

    public class StreamChannel : IChannel
    {
        [JsonProperty("num")]
        public string ChannelNumber { get; set; }

        [JsonProperty("epg_channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stream_icon")]
        public string LogoUrl { get; set; }

        [JsonProperty("stream_id")]
        public string StreamId { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; } // Single category ID

        private List<int> category_ids;
        [JsonProperty("category_ids")]
        public List<int> CategoryIds
        {
            get
            {
                if (category_ids == null)
                {
                    if (!string.IsNullOrEmpty(CategoryId))
                    {
                        category_ids = new List<int>() { int.Parse(CategoryId) };
                    }
                    else
                    {
                        category_ids = new List<int>();
                    }
                }
                return category_ids;
            }
            set
            {
                category_ids = value;
            }
        } // List of category IDs (some channels belong to more than one category)

        [JsonProperty("stream_type")]
        public string StreamType { get; set; }

        public string StreamID => StreamId;
        public string CategoryName { get; set; }

        public string Streamurl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/live/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamId}.ts";
            }
        }

        public string DisplayName => Name;
        public string IconUrl => LogoUrl;
        public string StreamUrl => Streamurl;
        public string Title => Name;
        public string Description => Name;
        public enumCategories Category => enumCategories.Live;

    }


}
