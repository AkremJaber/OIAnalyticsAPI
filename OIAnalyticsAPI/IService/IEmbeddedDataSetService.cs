using Microsoft.PowerBI.Api;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
    public interface IEmbeddedDataSetService
    {
        string GetAccessToken();
        PowerBIClient GetPowerBiClient();
        Task<EmbeddedDataSetViewModel> GetDataSet(string CCC_WorkspaceId, string DataSetId);
    }
}
