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
        private readonly IConfiguration Configuration;



        public EmbeddedReportService(IPowerBIService pbi, IConfiguration Configuration)
        {
            this.pbi = pbi;
            this.Configuration = Configuration;
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
            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.Edit, datasetId);
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

            return "Report deleted successfully";
        }

        public async Task ExportReport(string ExportType,string CCC_WorkspaceId,string ReportId,string ExportName, FileFormat ExportFileFormat)
        {
            switch (ExportType)
            {
                case "pdf":
                    ExportFileFormat = FileFormat.PDF;
                    break;
                case "pptx":
                    ExportFileFormat = FileFormat.PPTX;
                    break;
                case "png":
                    ExportFileFormat = FileFormat.PNG;
                    break;
                default:
                    throw new ApplicationException("Power BI reports do not support exort to " + ExportType);

            }
            powerBIClient = pbi.GetPowerBiClient();
            Guid WSID = new Guid(CCC_WorkspaceId);
            Guid RepId = new Guid(ReportId);
            var exportRequest = new ExportReportRequest
            {
                Format = ExportFileFormat,
                PowerBIReportConfiguration = new PowerBIReportExportConfiguration
                {
                    Settings = new ExportReportSettings
                    {
                        IncludeHiddenPages = false
                    }
                }
            };

            //if (ExportFilter != "")
            //{
            //    exportRequest.PowerBIReportConfiguration.ReportLevelFilters =
            //      new List<ExportFilter>() {
            //new ExportFilter { Filter = ExportFilter }
            //      };
            //}

            var export = await powerBIClient.Reports.ExportToFileInGroupAsync(WSID, RepId, exportRequest);

            string exportId = export.Id;
            do
            {
                System.Threading.Thread.Sleep(10000);
                export = await powerBIClient.Reports.GetExportToFileStatusInGroupAsync(WSID, RepId, exportId);
                Console.WriteLine(" - Export status: " + export.PercentComplete.ToString() + "% complete");
            } while (export.Status != ExportState.Succeeded && export.Status != ExportState.Failed);

            if (export.Status == ExportState.Failed)
            {
                Console.WriteLine("Export failed!");
            }

            if (export.Status == ExportState.Succeeded)
            {
                string ExportFolderPath = Configuration["ExportPath:export"];
                string FileName = ExportName + export.ResourceFileExtension.ToLower();
                string FilePath = ExportFolderPath + FileName;

                Console.WriteLine(" - Saving exported file to " + FilePath);
                var exportStream = await powerBIClient.Reports.GetFileOfExportToFileInGroupAsync(WSID, RepId, exportId);
                FileStream fileStream = File.Create(FilePath);
                exportStream.CopyTo(fileStream);
                fileStream.Close();
            }





            //powerBIClient = pbi.GetPowerBiClient();

            //// create export report request
            //var exportRequest = new ExportReportRequest
            //{
            //    Format = ExportFileFormat
            //};

            //Guid WSID = new Guid(CCC_WorkspaceId);
            //Guid RepId = new Guid(ReportId);

            //// call ExportToFileInGroup to start async export job
            //Export export = powerBIClient.Reports.ExportToFileInGroup(WSID, RepId, exportRequest);
            //string exportId = export.Id;

            //do
            //{
            //    //poll every 10 seconds to see if export job has completed
            //    System.Threading.Thread.Sleep(10000);

            //    export = await powerBIClient.Reports.GetExportToFileStatusInGroupAsync(WSID, RepId, exportId);

            //    // continue to poll until export job status is equal to Suceeded or Failed
            //} 
            //while (export.Status != ExportState.Succeeded && export.Status != ExportState.Failed);

            //if (export.Status == ExportState.Failed)
            //{
            //    // deal with failure
            //    Console.WriteLine("Export failed!");
            //}

            //if (export.Status == ExportState.Succeeded)
            //{
            //    // retreive file name extension from export object to construct file name
            //    string ExportFolderPath = Configuration["ExportPath:export"];
            //    string FileName = ExportName + export.ResourceFileExtension.ToLower();
            //    string FilePath = ExportFolderPath + FileName;

            //    // get export stream with file
            //    var exportStream = await powerBIClient.Reports.GetFileOfExportToFileInGroupAsync(WSID, RepId, exportId);

            //    // save exported file stream to local file
            //    FileStream fileStream = File.Create(FilePath);
            //    exportStream.CopyTo(fileStream);
            //    fileStream.Close();
            //}
        }
    }
}
