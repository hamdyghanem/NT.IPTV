using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Models.Channel;
using System.Runtime.CompilerServices;

namespace NT.IPTV.Utilities
{
    public class AppSettings
    {
        public string VlcLocationPath { set; get; } = string.Empty;
        public string LastProfile { set; get; } = string.Empty;
        public int ThumbnailSize { set; get; } = 100;
    }
}
