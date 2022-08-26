using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api;
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
            var cccTHP = System.Guid.NewGuid().ToString();
            var xobj = "<Key><T>CCCTenantsHasPersons</T><P>" + cccTHP + "</P></Key>";
            TenantsHasPersons thp = new TenantsHasPersons();
            thp.UID_CCCTenantsHasPersons = cccTHP;
            thp.CCC_UIDPerson = UID_Person;
            thp.CCC_UIDTenant = UID_Tenant;
            thp.XObjectKey = xobj;
            dbContext.CCCTenantsHasPersons.Add(thp);
            await dbContext.SaveChangesAsync();
            return thp;
        }

        public async Task<TenantsHasPersons> GetTHP(string UID_CCCTenantsHasPersons)
        {
            var thp = await dbContext.CCCTenantsHasPersons.Where(thp => thp.UID_CCCTenantsHasPersons == UID_CCCTenantsHasPersons).FirstOrDefaultAsync();
            return thp;
        }

        public async Task<string> DeleteTenantsHasPersons(string UID_CCCTenantsHasPersons)
        {
           TenantsHasPersons tenanthaspersons = await GetTHP(UID_CCCTenantsHasPersons);
           dbContext.CCCTenantsHasPersons.Remove(tenanthaspersons);
           await dbContext.SaveChangesAsync();
           return "Assignment deleted succesfully";
        }
    }
}

