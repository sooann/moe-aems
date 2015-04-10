using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe_aems.Models
{
    public partial class EntitiesNonLL : Entities
    {
        public EntitiesNonLL()
        {
            base.Configuration.ProxyCreationEnabled = false;
        }
    }
}
