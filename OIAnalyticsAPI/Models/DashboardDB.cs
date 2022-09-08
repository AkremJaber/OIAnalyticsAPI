using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class DashboardDB
    {
        [Key]
        public string UID_CCCDashboard { get; set; }
        public string CCC_WorkspaceId { get; set; }
        public string CCC_DashboardId { get; set; }
        public string CCC_DisplayName { get; set; }
        public string CCC_EmbedURL { get; set; }
        public bool CCC_IsReadOnly { get; set; }
        public string XObjectKey { get; set; }
    }
}
