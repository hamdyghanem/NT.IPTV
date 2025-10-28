using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Models.Items.Channesl;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Channel
{

    public class StreamSeries : IChannel
    {
        [JsonProperty("num")]
        public string num { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("series_id")]
        public string series_id { get; set; }

        [JsonProperty("cover")]
        public string CoverUrl { get; set; }

        [JsonProperty("plot")]
        public string Plot { get; set; }

        [JsonProperty("category_id")]
        public string category_id { get; set; } // Single category ID

        [JsonProperty("episode_run_time")]
        public string EpisodeRuntime { get; set; }

        [JsonProperty("youtube_trailer")]
        public string YoutubeTrailer { get; set; } = string.Empty;

        [JsonProperty("backdrop_path")]
        public string[] BackdropPath { get; set; } 

        [JsonProperty("rating_5based")]
        public string Rating5based { get; set; } = string.Empty;

        [JsonProperty("rating")]
        public string rating { get; set; } = string.Empty;

        [JsonProperty("last_modified")]
        public string LastModified { get; set; } = string.Empty;

        [JsonProperty("releaseDate")]
        public string releaseDate { get; set; } = string.Empty;


        [JsonProperty("added")]
        public string added { get; set; } = string.Empty;

        [JsonProperty("director")]
        public string Director { get; set; } = string.Empty;

        [JsonProperty("genre")]
        public string Genre { get; set; } = string.Empty;
        public string Streamurl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamID}.ts";
            }
        }

        public string ID=> series_id;
        public string CategoryID => category_id;
        public string Rating => rating;
        public string Title => Name;
        public string ReleaseDate => string.IsNullOrEmpty( releaseDate)? releaseDate: added;
        public string Description => Plot;
        public string IconUrl => CoverUrl;
        public string StreamID => series_id;
        public string StreamUrl => Streamurl;
        public enumCategories Category => enumCategories.Series;
        public enumItemType ItemType => enumItemType.Channel;
        private bool favorite = false;
        public bool Favorite { get => favorite; set => favorite = value; }
        private bool _hasNewEpisodes;
        public bool HasNewEpisodes { get => _hasNewEpisodes; set => _hasNewEpisodes = value; }
        public override string ToString()
        {
            return Name;
        }
    }
}
