using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class ReportRequest
    {
        public string Name { get; set; }
        public string CCC_WorkspaceId { get; set; }
        public string ReportId { get; set; }
    }
}
