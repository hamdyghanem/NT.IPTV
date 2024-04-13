using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.StreamObject
{

    public interface IMovie
    {
        enumCategories Category { get; }
        string Name { get; }
        string IconUrl { get; }
        string[] BackdropPath { get;  }
        string Duration { get; }
        
        string Cast { get; }
        string Genre { get; }
        string Director { get; }
        
        string Plot { get; }
        string StreamID { get; }
        string StreamUrl { get; }
        string ContainerExtension { get; }
        string YoutubeTrailer { get; }
        
    }

}
