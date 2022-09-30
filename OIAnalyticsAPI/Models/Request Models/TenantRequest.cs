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
        public string CCC_WorkspaceId { get; set; }
        public string groupUserAccessRight { get; set; }
        public string email { get; set; }


        public List<AddGroupModel> aadUser { get; set; }
    }

}
