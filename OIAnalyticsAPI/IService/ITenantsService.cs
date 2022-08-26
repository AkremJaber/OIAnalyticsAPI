using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface ITenantsService
    {
        string GetAccessToken();
        Dataset GetDataset(PowerBIClient pbiClient, Guid WorkspaceId, string DatasetName);
        PowerBIClient GetPowerBiClient();
        IList<Tenant> GetTenants();
        Task<Tenant> OnboardNewTenant(string name);
        void PublishPBIX(PowerBIClient pbiClient, Guid WorkspaceId, string PbixFilePath, string ImportName);
        Task<Tenant> DeleteWorkspace(string CCC_WorkspaceId);
        Task<Tenant> GetTenant(string CCC_WorkspaceId);
        Task<Tenant> GetTenantByUID(string UID_CCCTenants);
        Task<Tenant> CreateNewTenant(string name, PersonDictionary personDictionary);
        Task UpdateOneUserTenant(string CCC_WorkspaceId, string email);
        Task UpdateDictUserTenant(string CCC_WorkspaceId, PersonDictionary personDictionary);
    }

}