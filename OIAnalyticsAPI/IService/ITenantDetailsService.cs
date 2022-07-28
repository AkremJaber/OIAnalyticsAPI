using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
   public interface ITenantDetailsService
    {
        string GetAccessToken();
        PowerBIClient GetPowerBiClient();
        Task<TenantDetails> GetTenantDetails(string CCC_WorkspaceId);
    }
}
