﻿using Microsoft.Extensions.Configuration;
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
        private readonly IPowerBIService pbi;
        private PowerBIClient powerBIClient;


        public EmbeddedDataSetService(IPowerBIService pbi)
        {
            this.pbi = pbi;
        }
        public async Task<EmbeddedDataSetViewModel> GetDataSet(string CCC_WorkspaceId, string DataSetId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            var dataset = await powerBIClient.Datasets.GetDatasetInGroupAsync(WorkspaceId, DataSetId);

            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View);
            var embedTokenResponse = await powerBIClient.Datasets.GenerateTokenAsync(WorkspaceId, DataSetId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var datasetView = new EmbeddedDataSetViewModel
            {
                id = dataset.Id,
                name=dataset.Name,
                isRefreshable=dataset.IsRefreshable.ToString(),
            };
            return datasetView;
        }
        public async Task DeleteDataset(string CCC_WorkspaceId, string DataSetId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            await powerBIClient.Datasets.DeleteDatasetAsync(WorkspaceId, DataSetId);

        }
    }
}
