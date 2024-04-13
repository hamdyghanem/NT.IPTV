using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Models.Channel;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.StreamObject
{

    public class Movie : IMovie
    {
        [JsonProperty("info")]
        public MovieInfo MovieInfo { get; set; }
        [JsonProperty("movie_data")]
        public MovieData MovieData { get; set; }

        public enumCategories Category => enumCategories.Movies;

        public string Name => MovieData.Name;


        public string IconUrl => MovieInfo.MovieImage;
        public string[] BackdropPath => MovieInfo.BackdropPath;

        public string Duration => MovieInfo.Duration;

        public string Plot => MovieInfo.Plot;
        public string Cast => MovieInfo.Cast;
        public string Genre => MovieInfo.Genre;
        public string Director => MovieInfo.Director;

        public string StreamID => MovieData.StreamID;

        public string StreamUrl => MovieData.StreamUrl;
        public string ContainerExtension => MovieData.ContainerExtension;
        public string YoutubeTrailer => MovieInfo.youtube_trailer;

    }
    public class MovieInfo
    {
        [JsonProperty("movie_image")]
        public string MovieImage { get; set; }

        [JsonProperty("backdrop")]
        public string backdrop { get; set; }

        [JsonProperty("tmdb_id")]
        public string tmdb_id { get; set; }

        [JsonProperty("plot")]
        public string Plot { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }
        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("youtube_trailer")]
        public string youtube_trailer { get; set; }

        [JsonProperty("cast")]
        public string Cast { get; set; }

        [JsonProperty("backdrop_path")]
        public string[] BackdropPath { get; set; }
        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("duration_secs")]
        public string Duration_secs { get; set; }
        [JsonProperty("movie_data")]
        public MovieData MovieData { get; set; }

    }

    public class MovieData
    {
        [JsonProperty("stream_id")]
        public string StreamID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("container_extension")]
        public string ContainerExtension { get; set; }

        [JsonProperty("custom_sid")]
        public string custom_sid { get; set; }

        [JsonProperty("added")]
        public string added { get; set; }

        [JsonProperty("tags")]
        public string tags { get; set; }
        public string StreamUrl
        {
            get
            {
                return $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.PlayerInfo.server_info.url}:{clsCore.PlayerInfo.server_info.port}/movie/{clsCore.currentUser.UserName}/{clsCore.currentUser.Password}/{StreamID}.{ContainerExtension}";
            }
        }
    }

}
