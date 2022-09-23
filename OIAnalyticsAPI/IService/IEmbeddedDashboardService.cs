using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface IEmbeddedDashboardService
    {
        Task<EmbeddedDashboardViewModel> GetDashboard(string CCC_WorkspaceId, string DashboardId);
        Task<DashboardDB> PostDashboardInGrp(string CCC_WorkspaceId, string name);
        Task<AllDashboards> GetAllDashboards();
        Task<Dashboards> GetDash(string CCC_WorkspaceId);
        Task<DashboardDB> GetDashbyUID(string UID_CCCDashboard);
        Task<string> DeleteDashboard(string UID_CCCDashboard);
    }
}
