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
        private readonly ITenantsService tenantsService;
        private readonly IPowerBIService pbi;
        private PowerBIClient powerBIClient;
        public OIAnalyticsDBconfig dbContext;


        public EmbeddedDashboardService(IPowerBIService pbi, OIAnalyticsDBconfig dbContext, ITenantsService tenantsService)
        {
            this.tenantsService = tenantsService;
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
            DashboardDB dashboard = new DashboardDB();
            AddDashboardRequest request = new AddDashboardRequest(name);
            Guid WSID = new Guid(CCC_WorkspaceId);
            Dashboard dash = await powerBIClient.Dashboards.AddDashboardInGroupAsync(WSID, request);

            dashboard.CCC_DashboardId = dash.Id.ToString();
            dashboard.CCC_DisplayName = name;
            dashboard.CCC_IsReadOnly = dash.IsReadOnly.Value;
            Tenant tenant = await tenantsService.GetTenant(CCC_WorkspaceId);
            dashboard.CCC_WorkspaceId = tenant.UID_CCCTenants;
            var ccc_uid_Dash = System.Guid.NewGuid().ToString();
            dashboard.UID_CCCDashboard = ccc_uid_Dash;
            var xobj = "<Key><T>CCCDasboard</T><P>" + dashboard.UID_CCCDashboard + "</P></Key>";
            dashboard.XObjectKey = xobj;
            dashboard.CCC_EmbedURL = dash.EmbedUrl;
            dbContext.CCCDashboard.Add(dashboard);
            dbContext.SaveChanges();
            return dashboard;
        }

        public async Task<AllDashboards> GetAllDashboards()
        {
            powerBIClient = pbi.GetPowerBiClient();
            return new AllDashboards
            {
                Dashboard = powerBIClient.Dashboards.GetDashboardsAsAdmin().Value
            };
        }
    }
}

