using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
   public interface IEmbeddedReportService
    {
        Task<EmbeddedReportViewModel> GetReport(string CCC_WorkspaceId, string ReportId);
        Task<EmbeddedReportViewModel> CloneReport(string name, string CCC_WorkspaceId, string ReportId);
        Task<string> DeleteReport(string CCC_WorkspaceId, string ReportId);
        Task ExportReport(string CCC_WorkspaceId, string ReportId);
    }
}
