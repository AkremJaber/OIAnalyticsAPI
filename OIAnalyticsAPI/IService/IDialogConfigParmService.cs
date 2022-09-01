using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface IDialogConfigParmService
    {
        IList<DialogConfigParm> GetParameters();
        Task<string> GetConfValue(string ConfigParm);

    }
}
