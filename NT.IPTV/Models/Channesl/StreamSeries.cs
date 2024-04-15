using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
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
        public string Number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("series_id")]
        public string SeriesId { get; set; }

        [JsonProperty("cover")]
        public string CoverUrl { get; set; }

        [JsonProperty("plot")]
        public string Plot { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; } // Single category ID

        [JsonProperty("episode_run_time")]
        public string EpisodeRuntime { get; set; }

        [JsonProperty("youtube_trailer")]
        public string YoutubeTrailer { get; set; }

        [JsonProperty("backdrop_path")]
        public string[] BackdropPath { get; set; }

        [JsonProperty("rating_5based")]
        public string Rating5based { get; set; }

        [JsonProperty("rating")]
        public string rating { get; set; }

        [JsonProperty("last_modified")]
        public string LastModified { get; set; }

        [JsonProperty("releaseDate")]
        public string releaseDate { get; set; }
        

        [JsonProperty("added")]
        public string added { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }
        public string Streamurl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamID}.ts";
            }
        }

        public string Rating => rating;
        public string Title => Name;
        public string ReleaseDate => string.IsNullOrEmpty( releaseDate)? releaseDate: added;
        public string Description => Plot;
        public string IconUrl => CoverUrl;
        public string StreamID => SeriesId;
        public string StreamUrl => Streamurl;
        public enumCategories Category => enumCategories.Series;

    }
}
