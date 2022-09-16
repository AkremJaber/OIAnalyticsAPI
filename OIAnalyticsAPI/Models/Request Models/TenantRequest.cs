using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class TenantRequest
    {
        public string CCC_Name { get; set; }
        public string UID_Person { get; set; }
        public Dictionary<String,String> AadUser { get; set; }
    }

}
