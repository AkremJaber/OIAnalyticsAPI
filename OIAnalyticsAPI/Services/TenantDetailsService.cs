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
        private readonly IPowerBIService pbi;
        private PowerBIClient powerBIClient;

        public TenantDetailsService(IPowerBIService pbi)
        {
            this.pbi = pbi;  
        }

        public async Task<TenantDetails> GetTenantDetails(string CCC_WorkspaceId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WS_ID = new Guid(CCC_WorkspaceId);
            return  new TenantDetails
            {
                Datasets = powerBIClient.Datasets.GetDatasetsInGroup(WS_ID).Value,
                Reports = powerBIClient.Reports.GetReportsInGroup(WS_ID).Value,
                Dashboard = powerBIClient.Dashboards.GetDashboardsInGroup(WS_ID).Value,
            };
        }
    }
}
