using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface ITenantsHasPersonsService
    {
        IList<TenantsHasPersons> GetTenantsHasPersons();
        Task<TenantsHasPersons> AssignTenantToPerson(string UID_Person, string UID_Tenant);
        Task<TenantsHasPersons> GetTHP(string UID_CCCTenantsHasPersons);
        Task<TenantsHasPersons> DeleteTenantsHasPersons(string UID_CCCTenantsHasPersons);
    }
}
