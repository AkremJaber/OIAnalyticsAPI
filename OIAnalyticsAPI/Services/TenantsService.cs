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
        private PowerBIClient powerBIClient;
        private IDialogConfigParmService parmService;
        private readonly IPowerBIService pbi;
        private readonly IAssignPersonTenant assignPersonTenant;
        private readonly IConfiguration Configuration;
        public OIAnalyticsDBconfig dbContext;

        public TenantsService(IDialogConfigParmService parmService,IPowerBIService pbi,OIAnalyticsDBconfig dbContext,IConfiguration configuration,IPersonService ps,IAssignPersonTenant assignPersonTenant)
        {
            this.parmService = parmService;
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
            powerBIClient = pbi.GetPowerBiClient();
            GroupCreationRequest request = new GroupCreationRequest(name);
            // create new app workspace
            Tenant tenant = new();
            Group workspace = await powerBIClient.Groups.CreateGroupAsync(request);
            tenant.CCC_Name = name;
            tenant.CCC_WorkspaceId = workspace.Id.ToString();
            tenant.CCC_WorkspaceUrl = "https://app.powerbi.com/groups/"+workspace.Id.ToString() + "/";
            tenant.CCC_DatabaseServer = await parmService.GetConfValue("DatabaseServer");
            tenant.CCC_DatabaseName = await parmService.GetConfValue("DatabaseName");
            tenant.CCC_DatabaseUserName = await parmService.GetConfValue("DatabaseUserName");
            tenant.CCC_DatabaseUserPassword = await parmService.GetConfValue("DatabaseUserPassword");
            var ccc_uid_Tenant = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenants</T><P>" + ccc_uid_Tenant + "</P></Key>";
            tenant.UID_CCCTenants = ccc_uid_Tenant;
            tenant.XObjectKey = xobj;
            string adminUser = Configuration["DemoSettings:AdminUser"];
            // add user as new workspace admin  AddGroupUserAsync
            await assignPersonTenant.AddOneAdminUser(tenant.CCC_WorkspaceId,adminUser);
            // to publish a report in the workspace
            string pbixPath = @"C:\API\OIAnalyticsAPI\OIAnalyticsAPI\PBIX\test_Oneidentity_person.pbix";
            string importName = "Person_roles";
            PublishPBIX(powerBIClient, workspace.Id, pbixPath, importName);
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

        public async Task<string> DeleteWorkspace(string CCC_WorkspaceId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Tenant tenant = await GetTenant(CCC_WorkspaceId);
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            await powerBIClient.Groups.DeleteGroupAsync(workspaceIdGuid);
            dbContext.CCCTenants.Remove(tenant);
            dbContext.SaveChanges();
            return "Tenant deleted succesfully";
        }

        public async Task<Tenant> GetTenant(string CCC_WorkspaceId)
        {  
            var tenant = await dbContext.CCCTenants.Where(tenant => tenant.CCC_WorkspaceId == CCC_WorkspaceId).FirstOrDefaultAsync();
            return tenant;
        }

        public async Task<Tenant> GetTenantByUID(string UID_CCCTenants)
        {
            var tenant = await dbContext.CCCTenants.Where(tenant => tenant.UID_CCCTenants == UID_CCCTenants).FirstOrDefaultAsync();
            return tenant;
        }

        public async Task<Tenant> CreateNewTenant(string name, List<AddGroupModel> AadUserDict)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Tenant tenant = new Tenant();
            // create new app workspace
            GroupCreationRequest request = new GroupCreationRequest(name);
            // Group workspace = await pbiClient.Admin.;
            Group workspace = await powerBIClient.Groups.CreateGroupAsync(request);
            tenant.CCC_Name = name;
            tenant.CCC_WorkspaceId = workspace.Id.ToString();
            tenant.CCC_WorkspaceUrl = "https://app.powerbi.com/groups/" + workspace.Id.ToString() + "/";
            tenant.CCC_DatabaseServer = await parmService.GetConfValue("DatabaseServer");
            tenant.CCC_DatabaseName = await parmService.GetConfValue("DatabaseName");
            tenant.CCC_DatabaseUserName = await parmService.GetConfValue("DatabaseUserName");
            tenant.CCC_DatabaseUserPassword = await parmService.GetConfValue("DatabaseUserPassword");
            var ccc_uid_Tenant = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenants</T><P>" + ccc_uid_Tenant + "</P></Key>";
            tenant.UID_CCCTenants = ccc_uid_Tenant;
            tenant.XObjectKey = xobj;
            string adminUser = Configuration["DemoSettings:AdminUser"];
            // add user as new workspace admin  AddGroupUserAsync
            await assignPersonTenant.UpdateGroupUser(tenant.CCC_WorkspaceId, AadUserDict);

            string pbixPath = @"C:\API\OIAnalyticsAPI\OIAnalyticsAPI\PBIX\test_Oneidentity_person.pbix";
            string importName = "Person_roles";
            PublishPBIX(powerBIClient, workspace.Id, pbixPath, importName);           
            dbContext.CCCTenants.Add(tenant);
            dbContext.SaveChanges();
            return tenant;
        }

        public async Task<GroupUsers> GetGrpUsers(string CCC_WorkspaceId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            var grp = await powerBIClient.Groups.GetGroupUsersAsync(workspaceIdGuid);
            return grp;
        }

        //public async Task UpdateOneUserTenant(string CCC_WorkspaceId, string email)
        //{
        //    await assignPersonTenant.UpdateOneAdminUser(CCC_WorkspaceId, email); 
        //}

        //public async Task UpdateDictUserTenant(string CCC_WorkspaceId, string UID_Person)
        //{
        //    await assignPersonTenant.UpdateDictAdminUser(CCC_WorkspaceId, UID_Person);
        //}
    }
}
