using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Models.Channel;
using NT.IPTV.Models.StreamObject;
using System.Runtime.CompilerServices;

namespace NT.IPTV.Utilities
{
    public class AppSettings
    {

        
        public string VlcLocationPath { set; get; }
        public string LastProfile { set; get; }
        #region "Favorites ...
        public List<string> FavoritChannelsCategory { set; get; }=new List<string>();
        public List<string> FavoritMoviesCategory { set; get; } = new List<string>();
        public List<string> FavoritSeriesCategory { set; get; } = new List<string>();
        public List<string> FavoritChannels { set; get; }=new List<string>();
        public List<string> FavoritMovies { set; get; } = new List<string>();
        public List<string> FavoritSeries { set; get; } = new List<string>();

        #endregion
    }
}