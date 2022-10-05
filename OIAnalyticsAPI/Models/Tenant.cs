using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class Tenant
    {   
        [Key]
        public string CCC_WorkspaceId { get; set; }
        public string CCC_Name { get; set; }
        public string CCC_WorkspaceUrl { get; set; }
        public string CCC_DatabaseServer { get; set; }
        public string CCC_DatabaseName { get; set; }
        public string CCC_DatabaseUserName { get; set; }
        public string CCC_DatabaseUserPassword { get; set; }
        public string UID_CCCTenants { get; set; }
        public string XObjectKey { get; set; }

    }
}
