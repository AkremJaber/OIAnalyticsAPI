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
        EmbeddedDashboardViewModel GetDashboard(string CCC_WorkspaceId, string DashboardId);
        EmbeddedDashboardViewModel PostDashboardInGrp(string CCC_WorkspaceId, string name);
    }
}
