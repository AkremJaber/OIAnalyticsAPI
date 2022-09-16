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
        private readonly IPowerBIService pbi;
        private readonly IPersonService personService;
        private readonly IAADUser ADUserService;
        private PowerBIClient powerBIClient;


        public AssignPersonTenant(IPowerBIService pbi, IPersonService personService, IAADUser ADUserService)
        {
            this.personService = personService;
            this.pbi = pbi;
            this.ADUserService = ADUserService;
        }

        public async Task AddOneAdminUser(string CCC_WorkspaceId,string email)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            if (!string.IsNullOrEmpty(email))
            {
                await powerBIClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
                {
                    EmailAddress = email,
                    GroupUserAccessRight = "Admin"
                });
            }           
        }

        //public async Task AddDictAdminUser(string CCC_WorkspaceId,PersonDictionary personDictionary)
        //{
        //    powerBIClient = pbi.GetPowerBiClient();
        //    Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);

        //    foreach (var person in PersonDictionary.Persons)
        //    {
        //        await powerBIClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
        //        {
        //            EmailAddress = person.Key,
        //            GroupUserAccessRight = person.Value
        //        });
        //    }
        //}

        //public async Task UpdateOneAdminUser(string CCC_WorkspaceId,string email)
        //{
        //    powerBIClient = pbi.GetPowerBiClient();
        //    Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        await powerBIClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
        //        {
        //            Identifier = email,
        //            GroupUserAccessRight= "Admin",
        //            PrincipalType="None"
        //        });
        //    }
        //}

        public async Task UpdateDictAdminUser(string CCC_WorkspaceId, string UID_Person)
        {

            powerBIClient = pbi.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);
            var AdUser = await ADUserService.GetUserPrincipalName(UID_Person);
            var users = powerBIClient.Groups.GetGroupUsers(workspaceIdGuid).Value;

            foreach (var groupuser in users)
            {
                if (groupuser.Identifier.Equals(AdUser.UserPrincipalName))
                {
                    await powerBIClient.Groups.UpdateGroupUserAsync(workspaceIdGuid, new GroupUser
                    {
                        Identifier = AdUser.UserPrincipalName,
                        GroupUserAccessRight = "Admin",
                        PrincipalType = "None"
                    });
                }
            }
        }
        public async Task UpdateGroupUser(string CCC_WorkspaceId, List<TestModel> AadUserList )
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid workspaceIdGuid = new Guid(CCC_WorkspaceId);

            foreach (var Adperson in AadUserList)
            {
                //Person getPerson = await personService.GetPerson(Adperson.UID_Person);
                AADUser AADPerson = await ADUserService.GetUserPrincipalName(Adperson.UID_Person);
                string EmailAddress = AADPerson.UserPrincipalName;
                //var users = powerBIClient.Groups.GetGroupUsers(workspaceIdGuid);
                await powerBIClient.Groups.AddGroupUserAsync(workspaceIdGuid, new GroupUser
                       {
                            EmailAddress = EmailAddress,
                            GroupUserAccessRight = Adperson.accessRight,
                            PrincipalType = "User"
                        });
            }
        }

        //public async Task Update (string CCC_WorkspaceId)
    }
}

