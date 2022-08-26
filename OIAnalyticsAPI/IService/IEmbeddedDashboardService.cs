using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface IEmbeddedDashboardService
    {
        string GetAccessToken();
        PowerBIClient GetPowerBiClient();
        Task<EmbeddedDashboardViewModel> GetDashboard(string CCC_WorkspaceId, string DashboardId);
        Task<EmbeddedDashboardViewModel> PostDashboardInGrp(string CCC_WorkspaceId, string name);

        //Task<EmbeddedDashboardViewModel> GetDashboards(string CCC_WorkspaceId);
    }
}
