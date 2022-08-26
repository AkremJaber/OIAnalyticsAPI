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
        public TenantDetailsService(IPowerBIService pbi)
        {
            this.pbi = pbi;  
        }

        public async Task<TenantDetails> GetTenantDetails(string CCC_WorkspaceId)
        {
            PowerBIClient pbiClient = pbi.GetPowerBiClient();
            Guid WSID = new Guid(CCC_WorkspaceId);
            return  new TenantDetails
            {
                Datasets = pbiClient.Datasets.GetDatasetsInGroup(WSID).Value,
                Reports = pbiClient.Reports.GetReportsInGroup(WSID).Value,
                Dashboard = pbiClient.Dashboards.GetDashboardsInGroup(WSID).Value,
            };
        }
    }
}
