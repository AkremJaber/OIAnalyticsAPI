using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class TenantDetailsService : ITenantDetailsService
    {
        private readonly IConfiguration configuration;
        private string urlPowerBiServiceApiRoot { get; }
        private ITokenAcquisition tokenAcquisition { get; }
        public TenantDetailsService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
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
        public TenantDetails GetTenantDetails(string CCC_WorkspaceId)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Guid WSID = new Guid(CCC_WorkspaceId);
            return new TenantDetails
            {
                Datasets = pbiClient.Datasets.GetDatasetsInGroup(WSID).Value,
                Reports = pbiClient.Reports.GetReportsInGroup(WSID).Value,
                Dashboard = pbiClient.Dashboards.GetDashboardsInGroup(WSID).Value,
            };

        }
    }
}
