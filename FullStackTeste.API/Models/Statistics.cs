using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackTeste.API.Models
{
    public class Statistics
    {
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Page { get; set; }
        public dynamic Parameters { get; set; }
    }
}
