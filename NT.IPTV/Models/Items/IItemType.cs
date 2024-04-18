using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Items
{
    public interface IItemType
    {
        enumItemType ItemType { get; }
        string ID { get; }
        string Name { get; }
        string IconUrl { get; }

    }
}
