﻿using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Channel
{

    public class StreamVideo : IChannel
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

        public string DisplayName => Name;
        public string IconUrl => LogoUrl;
        public string Title => Name;
        public string Description => Name;
        public string StreamID => StreamId;
        public string StreamUrl => Streamurl;
        public enumCategories Category => enumCategories.Movies;

    }


}
