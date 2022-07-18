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
        EmbeddedReportViewModel GetReport(string CCC_WorkspaceId, string ReportId);
        PowerBIClient GetPowerBiClient();
        string GetAccessToken();
    }
}
