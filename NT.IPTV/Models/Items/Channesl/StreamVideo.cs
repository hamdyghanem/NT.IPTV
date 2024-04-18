using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Items.Channesl
{

    public class StreamVideo : IChannel
    {
        [JsonProperty("num")]
        public string num { get; set; }

        [JsonProperty("epg_channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stream_icon")]
        public string LogoUrl { get; set; }

        [JsonProperty("stream_id")]
        public string StreamId { get; set; }

        [JsonProperty("added")]
        public string added { get; set; }

        [JsonProperty("category_id")]
        public string category_id { get; set; } // Single category ID

        [JsonProperty("releaseDate")]
        public string releaseDate { get; set; }

        [JsonProperty("stream_type")]
        public string StreamType { get; set; }

        public string CategoryName { get; set; }

        public string Streamurl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamId}.ts";
            }
        }
        [JsonProperty("rating_5based")]
        public string Rating5based { get; set; }

        [JsonProperty("rating")]
        public string rating { get; set; }

        public string ID=> num;
        public string Rating => rating;
        public string IconUrl => LogoUrl;
        public string CategoryID => category_id;
        public string ReleaseDate => string.IsNullOrEmpty(releaseDate) ? releaseDate : added;
        public string Title => Name;
        public string Description => Name;
        public string StreamID => StreamId;
        public string StreamUrl => Streamurl;
        public enumCategories Category => enumCategories.Movies;
        public enumItemType ItemType => enumItemType.Channel;
        private bool favorite = false;
        public bool Favorite { get => favorite; set => favorite = value; }
        public override string ToString()
        {
            return Name;
        }
    }
}
