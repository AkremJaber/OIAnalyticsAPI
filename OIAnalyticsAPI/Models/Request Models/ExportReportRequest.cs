using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models.Request_Models
{
    public class ExportReportRequest
    {
        public string CCC_WorkspaceId { get; set; }
        public string ReportId { get; set; }
        public string ExportName { get; set; }
        public FileFormat ExportFileFormat { get; set; }
        public string ExportFilter { get; set; }


    }
}
