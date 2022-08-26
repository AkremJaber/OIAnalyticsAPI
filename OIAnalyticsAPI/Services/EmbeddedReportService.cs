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
    public class EmbeddedReportService : IEmbeddedReportService
    {
        private readonly IConfiguration configuration;
        private ITokenAcquisition tokenAcquisition { get; }
        private string urlPowerBiServiceApiRoot { get; }

        public EmbeddedReportService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
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

        public async Task<EmbeddedReportViewModel> GetReport(string CCC_WorkspaceId, string ReportId)
        {

            PowerBIClient pbiClient = GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid reportId = new Guid(ReportId);
            // call to Power BI Service API to get embedding data
            var report = await pbiClient.Reports.GetReportInGroupAsync(WorkspaceId,reportId);
            // generate read-only embed token for the report
            var datasetId = report.DatasetId;
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View, datasetId);
            var embedTokenResponse = await pbiClient.Reports.GenerateTokenAsync(WorkspaceId, reportId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            // return report embedding data to caller
            var t= new EmbeddedReportViewModel
            {
                ReportId = report.Id.ToString(),
                EmbedUrl = report.EmbedUrl,
                Name = report.Name,
                Token = embedToken
            };
            return t;
        }
    }
}
