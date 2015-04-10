using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe_aems.Models
{
    class JQGridSearch
    {
        public string groupOp { get; set; }
        public List<JQGridSearchRule> rules { get; set; }
        public List<JQGridSearch> groups { get; set; }
    }
}
