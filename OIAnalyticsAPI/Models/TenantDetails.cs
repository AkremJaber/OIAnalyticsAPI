using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class TenantDetails
    {
        public IList<Report> Reports { get; set; }
        public IList<Dataset> Datasets { get; set; }
        public IList<Dashboard> Dashboard { get; set; }
    }
}
