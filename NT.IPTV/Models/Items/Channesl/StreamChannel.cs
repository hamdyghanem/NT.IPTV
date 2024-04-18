using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NT.IPTV.Models.Items.Channesl
{

    public class StreamChannel : IChannel
    {
        [JsonProperty("num")]
        public string num { get; set; }

        [JsonProperty("epg_channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("stream_icon")]
        public string LogoUrl { get; set; }

        [JsonProperty("stream_id")]
        public string StreamId { get; set; }

        [JsonProperty("category_id")]
        public string category_id { get; set; } // Single category ID

        [JsonProperty("category_ids")]
        private List<int> category_ids;

        [JsonProperty("stream_type")]
        public string StreamType { get; set; }

        public string StreamID => StreamId;
        public string CategoryName { get; set; }

        public string Rating => string.Empty;
        public string Streamurl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/live/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamId}.ts";
            }
        }

        public string ID => num;
        public string Name => name;
        public string IconUrl => LogoUrl;
        public string StreamUrl => Streamurl;
        public string CategoryID => category_id;
        public string ReleaseDate => string.Empty;
        public string Description => name;
        public enumCategories Category => enumCategories.Live;
        public enumItemType ItemType => enumItemType.Channel;
        private bool favorite = false;
        public bool Favorite { get => favorite; set => favorite = value; }

        public override string ToString()
        {
            return Name;
        }
    }


}
