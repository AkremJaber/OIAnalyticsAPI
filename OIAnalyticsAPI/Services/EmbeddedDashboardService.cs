using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class EmbeddedDashboardService : IEmbeddedDashboardService
    {
        private readonly IPowerBIService pbi;

        public EmbeddedDashboardService(IPowerBIService pbi)
        {
            this.pbi = pbi;
        }
        public async Task<EmbeddedDashboardViewModel> GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            PowerBIClient pbiClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid dashboardId = new Guid(DashboardId);
            var dashboard = await pbiClient.Dashboards.GetDashboardInGroupAsync(WorkspaceId, dashboardId);

            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View);
            var embedTokenResponse = await pbiClient.Dashboards.GenerateTokenAsync(WorkspaceId, dashboardId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var dashView = new EmbeddedDashboardViewModel
            {
                DashboardId = dashboard.Id.ToString(),
                EmbedUrl = dashboard.EmbedUrl,
                Token = embedToken,
            };
            return dashView;
        }

        public async Task<EmbeddedDashboardViewModel> PostDashboardInGrp(string CCC_WorkspaceId, string name)
        {
            PowerBIClient pbiClient = pbi.GetPowerBiClient();
            AddDashboardRequest request = new AddDashboardRequest(name);
            Guid WSID = new Guid(CCC_WorkspaceId);
            Dashboard dash = await pbiClient.Dashboards.AddDashboardInGroupAsync(WSID, request);
            var dashView = new EmbeddedDashboardViewModel
            {
                Name=name,
                DashboardId = dash.Id.ToString(),
                EmbedUrl = dash.EmbedUrl,                
            };
            return dashView;
        }
    }
}
