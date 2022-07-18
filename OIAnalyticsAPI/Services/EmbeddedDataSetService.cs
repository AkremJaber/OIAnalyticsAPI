using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class EmbeddedDataSetService : IEmbeddedDataSetService
    {
        private readonly IConfiguration configuration;
        private ITokenAcquisition tokenAcquisition { get; }
        private string urlPowerBiServiceApiRoot { get; }
        public EmbeddedDataSetService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.tokenAcquisition = tokenAcquisition;
            this.urlPowerBiServiceApiRoot = configuration["PowerBi:ServiceRootUrl"];
        }
        public const string powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";

        public string GetAccessToken()
        {
            return this.tokenAcquisition.GetAccessTokenForAppAsync(powerbiApiDefaultScope).Result;
        }
        public PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }
        public EmbeddedDataSetViewModel GetDataSet(string CCC_WorkspaceId, string DataSetId)
        {
            PowerBIClient pbiClient = GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            //Guid datasetId = new Guid(DataSetId);
            var dataset = pbiClient.Datasets.GetDatasetInGroup(WorkspaceId, DataSetId);

            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View);
            var embedTokenResponse = pbiClient.Datasets.GenerateToken(WorkspaceId, DataSetId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var t = new EmbeddedDataSetViewModel
            {
                id = dataset.Id,
                name=dataset.Name,
                isRefreshable=dataset.IsRefreshable.ToString(),
            };
            return t;
        }

    }
}
