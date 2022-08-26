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
    public class AssignPersonTenant : IAssignPersonTenant
    {
        private readonly IConfiguration Configuration;
        private ITokenAcquisition TokenAcquisition { get; }
        private string urlPowerBiServiceApiRoot { get; }

        public AssignPersonTenant(ITokenAcquisition TokenAcquisition, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.TokenAcquisition = TokenAcquisition;
            this.urlPowerBiServiceApiRoot = configuration["PowerBi:ServiceRootUrl"];
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

        public async Task AddOneAdminUser(string CCC_WorkspaceId,string email)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            if (!string.IsNullOrEmpty(email))
            {
                await pbiClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
                {
                    EmailAddress = email,
                    GroupUserAccessRight = "Admin"
                });
            }           
        }

        public async Task AddDictAdminUser(string CCC_WorkspaceId,PersonDictionary personDictionary)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);

            foreach (var person in PersonDictionary.Persons)
            {
                await pbiClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
                {
                    EmailAddress = person.Key,
                    GroupUserAccessRight = person.Value
                });
            }
        }

        public async Task UpdateOneAdminUser(string CCC_WorkspaceId,string email)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            if (!string.IsNullOrEmpty(email))
            {
                await pbiClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
                {
                    EmailAddress = email,
                    GroupUserAccessRight= "Admin",
                    PrincipalType="None"
                });
            }
        }

        public async Task UpdateDictAdminUser(string CCC_WorkspaceId, PersonDictionary personDictionary)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            foreach (var person in PersonDictionary.Persons)
                {
                await pbiClient.Groups.UpdateGroupUserAsync(workspaceIdGuid, new GroupUser
                {
                    EmailAddress = person.Key,
                    GroupUserAccessRight = person.Value,
                });
            }
        }

    }
}

