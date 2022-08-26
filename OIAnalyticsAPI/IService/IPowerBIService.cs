using Microsoft.PowerBI.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
   public interface IPowerBIService
    {
        string GetAccessToken();
        PowerBIClient GetPowerBiClient();
    }
}
