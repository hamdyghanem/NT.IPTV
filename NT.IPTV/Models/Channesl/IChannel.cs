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
        string DisplayName { get; }
        string IconUrl { get; }
        string Title { get; }
        string Description { get; }
        string StreamID { get; }
        string StreamUrl { get; }
    }
}
