using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Items.Channesl
{
    public interface IChannel: IItemType
    {
        enumCategories Category { get; }
        public string CategoryID { get;  }
        public bool Favorite { get; set; }
        string Description { get; }
        string ReleaseDate { get; }
        string Rating { get; }
        string StreamID { get; }
        string StreamUrl { get; }

    }
}
