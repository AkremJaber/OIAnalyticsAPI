using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.DTOS;
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
        private readonly IConfiguration Configuration;
        private ITokenAcquisition TokenAcquisition { get; }
        public OIAnalyticsDBconfig dbContext;
        private string urlPowerBiServiceApiRoot { get; }
        public TenantsService(OIAnalyticsDBconfig dbContext, ITokenAcquisition TokenAcquisition, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.urlPowerBiServiceApiRoot = configuration["PowerBi:ServiceRootUrl"];
            this.TokenAcquisition = TokenAcquisition;
            this.dbContext = dbContext;           
        }
        public const string powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";
        public string GetAccessToken()
        {
            return this.TokenAcquisition.GetAccessTokenForAppAsync(powerbiApiDefaultScope).Result;
        }

        public PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        public IList<Tenant> GetTenants()
        {
            //Dim request = Deserialize(TenantObj, Json)
            //Dim tenantController = new TenantController()
            //  Dim response=   tenantController.GetTenants(request)

            //    Response Of type OIResponse()


            //    GetTenant(request Of type Request)
            //    {
            //    Dim tenantList = Session.GetCollection(Queryable(CCC_Tenant))
            //}
            return dbContext.CCCTenants
                   .Select(tenant => tenant)
                   .OrderBy(tenant => tenant.CCC_Name)
                   .ToList();
        }
        public Tenant OnboardNewTenant(string name)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Tenant tenant = new Tenant();
            // create new app workspace
            GroupCreationRequest request = new GroupCreationRequest(name);
            Group workspace = pbiClient.Groups.CreateGroup(request);
            tenant.CCC_Name = name;
            tenant.CCC_WorkspaceId = workspace.Id.ToString();
            tenant.CCC_WorkspaceUrl = "https://app.powerbi.com/groups/" + workspace.Id.ToString() + "/";
            tenant.CCC_DatabaseServer = "85.215.234.41\\SEAC1IM";
            tenant.CCC_DatabaseName = "SEACDEV01";
            tenant.CCC_DatabaseUserName = "PowerBi_User";
            tenant.CCC_DatabaseUserPassword = "Dublin42!";
            var ccc = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenants</T><P>" + ccc + "</P></Key>";
            tenant.UID_CCCTenants = ccc;
            tenant.XObjectKey = xobj;
            // add user as new workspace admin to make demoing easier
            string adminUser = Configuration["DemoSettings:AdminUser"];
            if (!string.IsNullOrEmpty(adminUser))
            {
                pbiClient.Groups.AddGroupUser(workspace.Id, new GroupUser
                {
                    EmailAddress = adminUser,
                    GroupUserAccessRight = "Admin"
                });
            }
            // upload sample PBIX file #1
            // string pbixPath = this.Env.WebRootPath + @"/PBIX/SalesReportTemplate.pbix";
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
        public Tenant DeleteWorkspace(string CCC_WorkspaceId)
        {

            PowerBIClient pbiClient = this.GetPowerBiClient();
            Tenant tenant = GetTenant(CCC_WorkspaceId);
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            pbiClient.Groups.DeleteGroup(workspaceIdGuid);
            dbContext.CCCTenants.Remove(tenant);
            dbContext.SaveChanges();
            return tenant;
        }
        public Tenant GetTenant(string CCC_WorkspaceId)
        {
            var tenant = dbContext.CCCTenants.Where(tenant => tenant.CCC_WorkspaceId == CCC_WorkspaceId).FirstOrDefault();
            return tenant;
        }
    }

}
