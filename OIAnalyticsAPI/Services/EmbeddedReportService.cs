using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class EmbeddedReportService : IEmbeddedReportService
    {
        private readonly IPowerBIService pbi;
        private PowerBIClient powerBIClient;



        public EmbeddedReportService(IPowerBIService pbi)
        {
            this.pbi = pbi;
        }

        public async Task<EmbeddedReportViewModel> GetReport(string CCC_WorkspaceId, string ReportId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid reportId = new Guid(ReportId);
            // call to Power BI Service API to get embedding data
            var report = await powerBIClient.Reports.GetReportInGroupAsync(WorkspaceId,reportId);
            // generate read-only embed token for the report
            var datasetId = report.DatasetId;
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View, datasetId);
            var embedTokenResponse = await powerBIClient.Reports.GenerateTokenAsync(WorkspaceId, reportId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            // return report embedding data to caller
            var reportView= new EmbeddedReportViewModel
            {
                ReportId = report.Id.ToString(),
                EmbedUrl = report.EmbedUrl,
                Name = report.Name,
                Token = embedToken
            };
            return reportView;
        }

        public async Task<EmbeddedReportViewModel> EditReport(string CCC_WorkspaceId, string ReportId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WorkspaceId = new Guid(CCC_WorkspaceId);
            Guid reportId = new Guid(ReportId);
            // call to Power BI Service API to get embedding data
            var report = await powerBIClient.Reports.GetReportInGroupAsync(WorkspaceId, reportId);
            // generate read-only embed token for the report
            var datasetId = report.DatasetId;
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.Create, datasetId);
            var embedTokenResponse = await powerBIClient.Reports.GenerateTokenAsync(WorkspaceId, reportId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            // return report embedding data to caller
            var reportView = new EmbeddedReportViewModel
            {
                ReportId = report.Id.ToString(),
                EmbedUrl = report.EmbedUrl,
                Name = report.Name,
                Token = embedToken
            };
            return reportView;
        }

        public async Task<EmbeddedReportViewModel> CloneReport(string name, string CCC_WorkspaceId, string ReportId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            CloneReportRequest request = new CloneReportRequest(name);
            Guid WSID = new Guid(CCC_WorkspaceId);
            Guid RepId = new Guid(ReportId);
            var clonedReport = await powerBIClient.Reports.CloneReportInGroupAsync(WSID, RepId, request);
            var datasetId = clonedReport.DatasetId;
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View, datasetId);
            var embedTokenResponse = await powerBIClient.Reports.GenerateTokenAsync(WSID, RepId, tokenRequest);
            var embedToken = embedTokenResponse.Token;
            var reportView = new EmbeddedReportViewModel
            {
                ReportId = clonedReport.Id.ToString(),
                EmbedUrl = clonedReport.EmbedUrl,
                Name = clonedReport.Name,
                Token = embedToken
            };
            return reportView;

        }
        public async Task<string> DeleteReport(string CCC_WorkspaceId, string ReportId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WSID = new Guid(CCC_WorkspaceId);
            Guid RepId = new Guid(ReportId);
            await powerBIClient.Reports.DeleteReportInGroupAsync(WSID, RepId);

            return "Report deleted succesfully";
        }

        public async Task<string> ExportReport(string CCC_WorkspaceId,string ReportId)
        {
            powerBIClient = pbi.GetPowerBiClient();
            Guid WSID = new Guid(CCC_WorkspaceId);
            Guid RepId = new Guid(ReportId);

            var req = new ExportReportRequest { Format = FileFormat.PDF };

          var export=  await powerBIClient.Reports.ExportToFileInGroupAsync(WSID,RepId,req);
            return export.Id;

        }

        
    }
}
