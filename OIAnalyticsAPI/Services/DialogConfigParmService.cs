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
    public class DialogConfigParmService:IDialogConfigParmService
    {
        public OIAnalyticsDBconfig dbContext;

        public DialogConfigParmService(OIAnalyticsDBconfig dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<DialogConfigParm> GetParameters()
        {
            return dbContext.DialogConfigParm
                   .Select(parms => parms)
                   .ToList();
        }
        public async Task<string> GetConfValue(string ConfigParm)
        {
            DialogConfigParm conf = await dbContext.DialogConfigParm.Where(dbinfo => dbinfo.ConfigParm == ConfigParm).FirstOrDefaultAsync();
            return conf.Value;
        }
    }
}
