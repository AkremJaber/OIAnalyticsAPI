using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class TenantsHasPersonsService : ITenantsHasPersonsService
    {
        public OIAnalyticsDBconfig dbContext;
        public TenantsHasPersonsService(OIAnalyticsDBconfig dbContext)
        {
            this.dbContext = dbContext;
        }
        public IList<TenantsHasPersons> GetTenantsHasPersons()
        {
            return dbContext.CCCTenantsHasPersons
                   .Select(tenant => tenant)
                   .OrderBy(tenant => tenant.UID_CCCTenantsHasPersons)
                   .ToList();
        }
        public async Task<TenantsHasPersons> AssignTenantToPerson(string UID_Person, string UID_Tenant)
        {
            var ccc = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenantsHasPersons</T><P>" + ccc + "</P></Key>";
            TenantsHasPersons thp = new TenantsHasPersons();
            thp.UID_CCCTenantsHasPersons = ccc;
            thp.CCC_UIDPerson = UID_Person;
            thp.CCC_UIDTenant = UID_Tenant;
            thp.XObjectKey = xobj;
            dbContext.CCCTenantsHasPersons.Add(thp);
            dbContext.SaveChangesAsync();
            return thp;
        }
    }
}

