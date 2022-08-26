using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class UpdateTenantRequest
    {
        public PersonDictionary PersonDictionary { get; set; }
        public string CCC_WorkspaceId { get; set; }
        public string email { get; set; }
    }
}
