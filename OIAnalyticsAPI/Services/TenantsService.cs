using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{    
    public class TenantsService : ITenantsService
    {
        private readonly IPersonService Ps;
        private readonly IPowerBIService pbi;
        private readonly IAssignPersonTenant assignPersonTenant;
        private readonly IConfiguration Configuration;
        public OIAnalyticsDBconfig dbContext;

        public TenantsService(IPowerBIService pbi,OIAnalyticsDBconfig dbContext,IConfiguration configuration,IPersonService ps,IAssignPersonTenant assignPersonTenant)
        {
            this.pbi = pbi;
            this.assignPersonTenant = assignPersonTenant;
            this.Ps = ps;
            this.Configuration = configuration;
            this.dbContext = dbContext;           
        }

        public IList<Tenant> GetTenants()
        {
            return dbContext.CCCTenants
                   .Select(tenant => tenant)
                   .OrderBy(tenant => tenant.CCC_Name)
                   .ToList();
        }
        public async Task<Tenant> OnboardNewTenant(string name)
        {
            PowerBIClient pbiClient = this.pbi.GetPowerBiClient();
            Tenant tenant = new Tenant();
            // create new app workspace
            GroupCreationRequest request = new GroupCreationRequest(name);
           // Group workspace = await pbiClient.Admin.;
            Group workspace = await pbiClient.Groups.CreateGroupAsync(request);
            tenant.CCC_Name = name;
            tenant.CCC_WorkspaceId = workspace.Id.ToString();
            tenant.CCC_WorkspaceUrl = "https://app.powerbi.com/groups/"+workspace.Id.ToString() + "/";
            tenant.CCC_DatabaseServer = "85.215.234.41\\SEAC1IM";
            tenant.CCC_DatabaseName = "SEACDEV01";
            tenant.CCC_DatabaseUserName = "PowerBi_User";
            tenant.CCC_DatabaseUserPassword = "Dublin42!";
            var cccTenant = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenants</T><P>" + cccTenant + "</P></Key>";
            tenant.UID_CCCTenants = cccTenant;
            tenant.XObjectKey = xobj;
            string adminUser = Configuration["DemoSettings:AdminUser"];
            // add user as new workspace admin  AddGroupUserAsync
            await assignPersonTenant.AddOneAdminUser(tenant.CCC_WorkspaceId,adminUser);
           
            string pbixPath = @"C:\API\OIAnalyticsAPI\OIAnalyticsAPI\PBIX\test_Oneidentity_person.pbix";
            string importName = "Person_roles";
            PublishPBIX(pbiClient, workspace.Id, pbixPath, importName);
            Dataset dataset = GetDataset(pbiClient, workspace.Id, importName);
            //  UpdateMashupParametersRequest req =
            //    new UpdateMashupParametersRequest(new List<UpdateMashupParameterDetails>() {
            //new UpdateMashupParameterDetails { Name = "DatabaseServer", NewValue = tenant.DatabaseServer },
            //new UpdateMashupParameterDetails { Name = "DatabaseName", NewValue = tenant.DatabaseName }
            //  });
            //  pbiClient.Datasets.UpdateParametersInGroup(workspace.Id, dataset.Id, req);
            //  PatchSqlDatasourceCredentials(pbiClient, workspace.Id, dataset.Id, tenant.DatabaseUserName, tenant.DatabaseUserPassword);
            //  pbiClient.Datasets.RefreshDatasetInGroup(workspace.Id, dataset.Id);
            dbContext.CCCTenants.Add(tenant);
            dbContext.SaveChanges();
            return tenant;
        }

        public void PublishPBIX(PowerBIClient pbiClient, Guid WorkspaceId, string PbixFilePath, string ImportName)
        {
            FileStream stream = new FileStream(PbixFilePath, FileMode.Open, FileAccess.Read);
            var import = pbiClient.Imports.PostImportWithFileInGroup(WorkspaceId, stream, ImportName);
            while (import.ImportState != "Succeeded")
            {
                import = pbiClient.Imports.GetImportInGroup(WorkspaceId, import.Id);
            }
        }

        public Dataset GetDataset(PowerBIClient pbiClient, Guid WorkspaceId, string DatasetName)
        {
            var datasets = pbiClient.Datasets.GetDatasetsInGroup(WorkspaceId).Value;
            foreach (var dataset in datasets)
            {
                if (dataset.Name.Equals(DatasetName))
                {
                    return dataset;
                }
            }
            return null;
        }

        public async Task<string> DeleteWorkspace(string CCC_WorkspaceId)
        {
            PowerBIClient pbiClient = this.pbi.GetPowerBiClient();
            Tenant tenant = await GetTenant(CCC_WorkspaceId);
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            await pbiClient.Groups.DeleteGroupAsync(workspaceIdGuid);
            dbContext.CCCTenants.Remove(tenant);
            dbContext.SaveChanges();
            return "Tenant deleted succesfully";
        }

        public async Task<Tenant> GetTenant(string CCC_WorkspaceId)
        {
            PowerBIClient pbiClient = pbi.GetPowerBiClient();
           
            var tenant = await dbContext.CCCTenants.Where(tenant => tenant.CCC_WorkspaceId == CCC_WorkspaceId).FirstOrDefaultAsync();
            return tenant;
        }

        public async Task<Tenant> GetTenantByUID(string UID_CCCTenants)
        {
            var tenant = await dbContext.CCCTenants.Where(tenant => tenant.UID_CCCTenants == UID_CCCTenants).FirstOrDefaultAsync();
            return tenant;
        }

        public async Task<Tenant> CreateNewTenant(string name,PersonDictionary personDictionary)
        {
            PowerBIClient pbiClient = pbi.GetPowerBiClient();
            Tenant tenant = new Tenant();
            // create new app workspace
            GroupCreationRequest request = new GroupCreationRequest(name);
            // Group workspace = await pbiClient.Admin.;
            Group workspace = await pbiClient.Groups.CreateGroupAsync(request);
            tenant.CCC_Name = name;
            tenant.CCC_WorkspaceId = workspace.Id.ToString();
            tenant.CCC_WorkspaceUrl = "https://app.powerbi.com/groups/" + workspace.Id.ToString() + "/";
            tenant.CCC_DatabaseServer = "85.215.234.41\\SEAC1IM";
            tenant.CCC_DatabaseName = "SEACDEV01";
            tenant.CCC_DatabaseUserName = "PowerBi_User";
            tenant.CCC_DatabaseUserPassword = "Dublin42!";
            var cccTenant = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenants</T><P>" + cccTenant + "</P></Key>";
            tenant.UID_CCCTenants = cccTenant;
            tenant.XObjectKey = xobj;
            string adminUser = Configuration["DemoSettings:AdminUser"];
            // add user as new workspace admin  AddGroupUserAsync
            await assignPersonTenant.AddDictAdminUser(tenant.CCC_WorkspaceId, personDictionary);

            string pbixPath = @"C:\API\OIAnalyticsAPI\OIAnalyticsAPI\PBIX\test_Oneidentity_person.pbix";
            string importName = "Person_roles";
            PublishPBIX(pbiClient, workspace.Id, pbixPath, importName);
            Dataset dataset = GetDataset(pbiClient, workspace.Id, importName);
            dbContext.CCCTenants.Add(tenant);
            dbContext.SaveChanges();
            return tenant;
        }

        public async Task UpdateOneUserTenant(string CCC_WorkspaceId, string email)
        {
            await assignPersonTenant.UpdateOneAdminUser(CCC_WorkspaceId, email); 
        }

        public async Task UpdateDictUserTenant(string CCC_WorkspaceId, PersonDictionary personDictionary)
        {
            await assignPersonTenant.UpdateDictAdminUser(CCC_WorkspaceId, personDictionary);
        }
    }
}
