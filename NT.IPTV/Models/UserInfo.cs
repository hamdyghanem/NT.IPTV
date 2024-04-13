using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models
{
    public class UserInfo
    {
        public string Name { get; set; }=string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;  
        public string Port { get; set; } = string.Empty;
        public bool UseHttps { get; set; }  
       
    }
}
