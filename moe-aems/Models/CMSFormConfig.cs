using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe_aems.Models
{
    public class CMSFormConfig
    {
        public string url { get; set; }
        public string name { get; set; }
        public string dataUrl { get; set; }
        public IList<CMSFormField> fields { get; set; }
        public CMSForm formsetting { get; set; }
    }
}
