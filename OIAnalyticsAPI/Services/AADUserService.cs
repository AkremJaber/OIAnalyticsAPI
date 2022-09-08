using Microsoft.EntityFrameworkCore;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class AADUserService : IAADUser
    {
        public OIAnalyticsDBconfig dbContext;
        public AADUserService(OIAnalyticsDBconfig dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<AADUser> GetAADUsers()
        {
            return dbContext.AADUser
                   .Select(aaduser => aaduser)
                   .ToList();
        }

        //public async Task<AADUser> GetAADUser(string UID_AADUser)
        //{
        //    var aaduser = await dbContext.AADUser.Where(user => user.UID_AADUser == UID_AADUser).FirstOrDefaultAsync();
        //    return aaduser;
        //}

        public async Task<AADUser> GetUserPrincipalName(string UID_Person)
        {
            var aaduserr = await dbContext.AADUser.Where(user => user.UID_Person == UID_Person).FirstOrDefaultAsync();
            return aaduserr;
        }

    }
}
