using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class AllDashboards
    {
        public IList<Dashboard> Dashboard { get; set; }
    }
}
