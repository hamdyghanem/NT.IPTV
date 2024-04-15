using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Channel
{
    public interface IChannel
    {
        enumCategories Category { get; }
        string IconUrl { get; }
        string Title { get; }
        string Description { get; }
        string ReleaseDate { get; }
        string Rating { get; }

        string StreamID { get; }
        string StreamUrl { get; }
    }
}
