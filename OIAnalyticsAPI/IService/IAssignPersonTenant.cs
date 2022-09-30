using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface IAssignPersonTenant
    {
        Task AddOneAdminUser(string CCC_WorkspaceId, string email);
        //Task AddDictAdminUser(string CCC_WorkspaceId,PersonDictionary personDictionary);
        //Task UpdateOneAdminUser(string CCC_WorkspaceId, string email);
        Task AddGrpUsers(string CCC_WorkspaceId, List<AddGroupModel> AadUserList, string groupUserAccessRight);
        Task UpdateGroupUser(string CCC_WorkspaceId, List<AddGroupModel> AadUserList);
    }
}
