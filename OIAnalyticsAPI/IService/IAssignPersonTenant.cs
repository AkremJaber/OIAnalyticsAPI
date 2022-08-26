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
        Task AddDictAdminUser(string CCC_WorkspaceId,PersonDictionary personDictionary);
        Task UpdateOneAdminUser(string CCC_WorkspaceId, string email);
        Task UpdateDictAdminUser(string CCC_WorkspaceId, PersonDictionary personDictionary);
    }
}
