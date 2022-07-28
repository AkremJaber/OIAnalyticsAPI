using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddedReportController : ControllerBase
    {
        public readonly IEmbeddedReportService embeddedReport;
        public readonly ITenantsService ts;
        public EmbeddedReportController (IEmbeddedReportService embeddedReport, ITenantsService ts)
        {
            this.embeddedReport = embeddedReport;
            this.ts = ts;
        }
        [HttpGet("{CCC_WorkspaceId}/{ReportId}")]
        
        public async Task<ActionResult<EmbeddedReportViewModel>> GetReport (string CCC_WorkspaceId,string ReportId)
        {
            try
            {
                var report = await embeddedReport.GetReport(CCC_WorkspaceId, ReportId);
                return report;
            }
            catch
            {
                if (await ts.GetTenant(CCC_WorkspaceId) == null)
                {
                    int err = 101;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
                else
                {
                    int err = 102;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
            }


        }

    }
}
