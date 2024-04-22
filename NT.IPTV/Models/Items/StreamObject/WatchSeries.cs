using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Items.StreamObject
{

    public class WatchSeries : IWatch
    {

        [JsonProperty("seasons")]
        public SeasonData[] SeasonsData { get; set; }
        [JsonProperty("info")]
        public SeriesInfo SeriesInfo { get; set; }
        [JsonProperty("episodes")]
        public Episodes Episodes { get; set; }
        public List<Season> Seasons { get; set; } = new List<Season>();

        public enumCategories Category => enumCategories.Series;
        public enumItemType ItemType => enumItemType.Watch;

        public string ID => SeriesInfo.tmdb_id;
        public string Name => SeriesInfo.Name;
        public string Duration => string.Empty;

        public string IconUrl => SeriesInfo.Cover;
        public string Cast => SeriesInfo.Cast;

        public string[] BackdropPath => SeriesInfo.BackdropPath;

        public string Plot => SeriesInfo.Plot;
        public string Genre => SeriesInfo.Genre;
        public string Director => SeriesInfo.Director;

        public string StreamID => string.Empty;

        public string StreamUrl => string.Empty;
        public string ContainerExtension => string.Empty;
        public string YoutubeTrailer => SeriesInfo.youtube_trailer;

    }
    public class SeriesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cover")]
        public string Cover { get; set; }

        [JsonProperty("backdrop")]
        public string backdrop { get; set; }

        [JsonProperty("tmdb_id")]
        public string tmdb_id { get; set; }

        [JsonProperty("plot")]
        public string Plot { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("youtube_trailer")]
        public string youtube_trailer { get; set; }

        [JsonProperty("cast")]
        public string Cast { get; set; }

        [JsonProperty("backdrop_path")]
        public string[] BackdropPath { get; set; }

        [JsonProperty("duration_secs")]
        public string Duration_secs { get; set; }
    }
    public class Season
    {
        public List<EpisodeData> Episodes { get; set; }
    }

    public class Episodes
    {
        [JsonProperty("1")]
        public List<EpisodeData> EpisodeData_1 { get; set; }
        [JsonProperty("2")]
        public List<EpisodeData> EpisodeData_2 { get; set; }
        [JsonProperty("3")]
        public List<EpisodeData> EpisodeData_3 { get; set; }
        [JsonProperty("4")]
        public List<EpisodeData> EpisodeData_4 { get; set; }
        [JsonProperty("5")]
        public List<EpisodeData> EpisodeData_5 { get; set; }
        [JsonProperty("6")]
        public List<EpisodeData> EpisodeData_6 { get; set; }
        [JsonProperty("7")]
        public List<EpisodeData> EpisodeData_7 { get; set; }
        [JsonProperty("8")]
        public List<EpisodeData> EpisodeData_8 { get; set; }
        [JsonProperty("9")]
        public List<EpisodeData> EpisodeData_9 { get; set; }
        [JsonProperty("10")]
        public List<EpisodeData> EpisodeData_10 { get; set; }
        [JsonProperty("11")]
        public List<EpisodeData> EpisodeData_11 { get; set; }
        [JsonProperty("12")]
        public List<EpisodeData> EpisodeData_12 { get; set; }
    }
    public class EpisodeInfo
    {
        [JsonProperty("duration_secs")]
        public string Duration_secs { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }
        [JsonProperty("movie_image")]
        public string movie_image { get; set; }
        [JsonProperty("plot")]
        public string plot { get; set; }

        [JsonProperty("releasedate")]
        public string releasedate { get; set; }

    }
    public class SeasonData
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("episode_count")]
        public int episode_count { get; set; }

        [JsonProperty("air_date")]
        public string air_date { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("season_number")]
        public int season_number { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("vote_average")]
        public string vote_average { get; set; }

        [JsonProperty("cover")]
        public string cover { get; set; }

        [JsonProperty("cover_big")]
        public string cover_big { get; set; }
    }
    public class EpisodeData
    {
        [JsonProperty("id")]
        public string StreamId { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }


        [JsonProperty("episode_num")]
        public string EpisodeNum { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("container_extension")]
        public string ContainerExtension { get; set; }

        [JsonProperty("custom_sid")]
        public string custom_sid { get; set; }

        [JsonProperty("added")]
        public string added { get; set; }

        [JsonProperty("info")]
        public EpisodeInfo Info { get; set; }
        public string StreamUrl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/series/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamId}.{ContainerExtension}";
            }
        }
    }
}
