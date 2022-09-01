using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
   public interface IEmbeddedReportService
    {
        Task<EmbeddedReportViewModel> GetReport(string CCC_WorkspaceId, string ReportId);
        Task<EmbeddedReportViewModel> CloneReport(string name, string CCC_WorkspaceId, string ReportId);
    }
}
