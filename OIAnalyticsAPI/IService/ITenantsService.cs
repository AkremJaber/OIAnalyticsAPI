using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.DTOS;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;

namespace OIAnalyticsAPI.IService
{
    public interface ITenantsService
    {
        string GetAccessToken();
        Dataset GetDataset(PowerBIClient pbiClient, Guid WorkspaceId, string DatasetName);
        PowerBIClient GetPowerBiClient();
        IList<Tenant> GetTenants();
        Tenant OnboardNewTenant(string name);
        void PublishPBIX(PowerBIClient pbiClient, Guid WorkspaceId, string PbixFilePath, string ImportName);
        Tenant DeleteWorkspace(string CCC_WorkspaceId);
        Tenant GetTenant(string CCC_WorkspaceId);
    }

}