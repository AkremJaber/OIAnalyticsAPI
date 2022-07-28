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
        private readonly IConfiguration configuration;
        private ITokenAcquisition tokenAcquisition { get; }
        private string urlPowerBiServiceApiRoot { get; }
        public EmbeddedDashboardService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.tokenAcquisition = tokenAcquisition;
            this.urlPowerBiServiceApiRoot = configuration["PowerBi:ServiceRootUrl"];
        }
        public const string powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";

        public string GetAccessToken()
        {
            return this.tokenAcquisition.GetAccessTokenForAppAsync(powerbiApiDefaultScope).Result;
        }
        public PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }
        public async Task<EmbeddedDashboardViewModel> GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            PowerBIClient pbiClient = GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid dashboardId = new Guid(DashboardId);
            var dashboard = await pbiClient.Dashboards.GetDashboardInGroupAsync(WorkspaceId, dashboardId);

            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View);
            var embedTokenResponse = await pbiClient.Dashboards.GenerateTokenAsync(WorkspaceId, dashboardId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var t = new EmbeddedDashboardViewModel
            {
                DashboardId = dashboard.Id.ToString(),
                EmbedUrl = dashboard.EmbedUrl,
                Token = embedToken,

            };
            return t;
        }
        public async Task<EmbeddedDashboardViewModel> PostDashboardInGrp(string CCC_WorkspaceId, string name)
        {
            PowerBIClient pbiClient = GetPowerBiClient();
            AddDashboardRequest request = new AddDashboardRequest(name);
            Guid WSID = new Guid(CCC_WorkspaceId);
            Dashboard dash = await pbiClient.Dashboards.AddDashboardInGroupAsync(WSID, request);
            var t = new EmbeddedDashboardViewModel
            {
                Name=name,
                DashboardId = dash.Id.ToString(),
                EmbedUrl = dash.EmbedUrl,
                
            };
            return t;

        }

        //public async Task<EmbeddedDashboardViewModel> GetDashboards(string CCC_WorkspaceId)
        //{
        //    PowerBIClient pbiClient = GetPowerBiClient();
        //    Guid WorkspaceId = new Guid(CCC_WorkspaceId);
        //    var dash = await pbiClient.Dashboards.GetDashboardsAsync(WorkspaceId);
            
            
        //    return dash;

        //}
        //public EmbeddedDashboardViewModel DeleteDashboardInGrp(string CCC_WorkspaceId, string DashboardId)
        //{
        //    PowerBIClient pbiClient = GetPowerBiClient();
        //    EmbeddedDashboardViewModel dash = GetDashboard(CCC_WorkspaceId, DashboardId);
        //    Guid WSID = new Guid(CCC_WorkspaceId);
             
        //}

        //public EmbeddedDashboardViewModel DeleteDashboard(string CCC_WorkspaceId, string DashboardId)
        //{

        //    PowerBIClient pbiClient = this.GetPowerBiClient();
        //    // EmbeddedDashboardViewModel dash = GetDashboard(CCC_WorkspaceId, DashboardId);
        //    Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
        //    Guid dashIdGuid = new Guid(DashboardId);
        //    pbiClient.Dashboards.GetDashboardInGroup(workspaceIdGuid, dashIdGuid)
        //}

    }
}
