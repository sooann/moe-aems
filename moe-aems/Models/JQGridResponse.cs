using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe_aems.Models
{
    class JQGridResponse<T>
    {
        public long total { get; set; }
        public long page { get; set; }
        public long records { get; set; }
        public IQueryable<T> rows { get; set; }
    }
}
