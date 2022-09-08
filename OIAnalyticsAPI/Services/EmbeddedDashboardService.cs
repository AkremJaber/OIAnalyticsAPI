using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using OIAnalyticsAPI.Configs;
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
        private PowerBIClient powerBIClient;
        public OIAnalyticsDBconfig dbContext;


        public EmbeddedDashboardService(IPowerBIService pbi, OIAnalyticsDBconfig dbContext)
        {
            this.dbContext = dbContext;
            this.pbi = pbi;
        }
        public async Task<EmbeddedDashboardViewModel> GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid dashboardId = new Guid(DashboardId);
            var dashboard = await powerBIClient.Dashboards.GetDashboardInGroupAsync(WorkspaceId, dashboardId);
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View);
            var embedTokenResponse = await powerBIClient.Dashboards.GenerateTokenAsync(WorkspaceId, dashboardId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var dashView = new EmbeddedDashboardViewModel
            {
                DashboardId = dashboard.Id.ToString(),
                EmbedUrl = dashboard.EmbedUrl,
                Token = embedToken,
            };
            return dashView;
        }

        public async Task<DashboardDB> PostDashboardInGrp(string CCC_WorkspaceId, string name)
        {
            powerBIClient = pbi.GetPowerBiClient();
            AddDashboardRequest request = new AddDashboardRequest(name);
            Guid WSID = new Guid(CCC_WorkspaceId);
            Dashboard dash = await powerBIClient.Dashboards.AddDashboardInGroupAsync(WSID, request);

            DashboardDB dashboard = new DashboardDB();
            dashboard.CCC_DashboardId = dash.Id.ToString();
            dashboard.CCC_DisplayName = name;
            dashboard.CCC_IsReadOnly = dash.IsReadOnly.Value;
            dashboard.CCC_WorkspaceId = CCC_WorkspaceId;
            dashboard.UID_CCCDashboard = dash.Id.ToString();
            var xobj = "<Key><T>CCCDasboard</T><P>" + dashboard.UID_CCCDashboard + "</P></Key>";
            dashboard.XObjectKey = xobj;
            dashboard.CCC_EmbedURL = dash.EmbedUrl;
            dbContext.CCCDashboard.Add(dashboard);
            dbContext.SaveChanges();

            var dashView = new DashboardDB
            {
                
                CCC_DisplayName=name,
                CCC_DashboardId = dash.Id.ToString(),
                CCC_WorkspaceId= CCC_WorkspaceId,
                CCC_EmbedURL = dash.EmbedUrl,                
            };
            return dashView;
        }
    }
}
