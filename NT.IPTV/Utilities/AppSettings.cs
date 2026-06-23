using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Models.Channel;
using System.Runtime.CompilerServices;

namespace NT.IPTV.Utilities
{
    public class AppSettings
    {
        public string ApplicationLanguage { get; set; }
        public string VlcLocationPath { set; get; } = string.Empty;
        public string LastProfile { set; get; } = string.Empty;
        public int ThumbnailSize { set; get; } = 100;
        /// <summary>Route all IPTV API calls and stream URLs through the Azure proxy to bypass ISP blocks.</summary>
        public bool UseAzureProxy { set; get; } = false;
        public string ProxyBaseUrl { set; get; } = "https://ntiptv.azurewebsites.net";
        /// <summary>Cache the full channel/movie/series catalog locally to enable instant startup.</summary>
        public bool EnableCatalogCache { set; get; } = true;
        /// <summary>How many hours the local catalog cache is considered fresh before a re-fetch is needed.</summary>
        public int CacheExpiryHours { set; get; } = 4;
        /// <summary>Skip the login screen and connect automatically using the LastProfile on startup.</summary>
        public bool AutoLogin { set; get; } = false;
        /// <summary>Playback positions keyed by stream URL (milliseconds), used for Continue Watching.</summary>
        public Dictionary<string, long> PlaybackPositions { set; get; } = new Dictionary<string, long>();
        /// <summary>Use the built-in player instead of launching VLC externally.</summary>
        public bool UseBuiltInPlayer { set; get; } = true;
        /// <summary>Use dark mode UI theme.</summary>
        public bool DarkMode { set; get; } = true;
    }
}
