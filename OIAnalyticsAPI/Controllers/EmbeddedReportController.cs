using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddedReportController : ControllerBase
    {
        public readonly IEmbeddedReportService embeddedReport;
        public EmbeddedReportController (IEmbeddedReportService embeddedReport)
        {
            this.embeddedReport = embeddedReport;
        }
        [HttpGet("{CCC_WorkspaceId}/{ReportId}")]
        public  EmbeddedReportViewModel GetReport (string CCC_WorkspaceId,string ReportId)
        {
            var report = embeddedReport.GetReport(CCC_WorkspaceId, ReportId);
            return report;

        }

    }
}
