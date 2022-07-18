using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class EmbeddedDashboardViewModel
    {
        public string DashboardId { get; set; }
        public string Name { get; set; }
        public string EmbedUrl { get; set; }
        public string Token { get; set; }
    }
}
