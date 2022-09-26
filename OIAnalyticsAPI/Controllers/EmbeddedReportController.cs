using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using OIAnalyticsAPI.Models.Request_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmbeddedReportController : ControllerBase
    {
        public readonly IEmbeddedReportService embeddedReport;
        public readonly ITenantsService tenantservice;
        public EmbeddedReportController (IEmbeddedReportService embeddedReport, ITenantsService tenantservice)
        {
            this.embeddedReport = embeddedReport;
            this.tenantservice = tenantservice;
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
                if (await tenantservice.GetTenant(CCC_WorkspaceId) == null)
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
        
        [HttpGet("{CCC_WorkspaceId}/{ReportId}")]
        public async Task<ActionResult<EmbeddedReportViewModel>> EditReport(string CCC_WorkspaceId, string ReportId)
        {
            try
            {
                var report = await embeddedReport.EditReport(CCC_WorkspaceId, ReportId);
                return report;
            }
            catch
            {
                if (await tenantservice.GetTenant(CCC_WorkspaceId) == null)
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

        [HttpPost]
        public async Task<ActionResult<EmbeddedReportViewModel>> CloneReport(ReportRequest repReq)
        {
            try
            {
                var clonedReport = await embeddedReport.CloneReport(repReq.Name, repReq.CCC_WorkspaceId, repReq.ReportId);
                return clonedReport;
            }
            catch
            {
                if (await tenantservice.GetTenant(repReq.CCC_WorkspaceId) == null)
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
        [HttpDelete("{CCC_WorkspaceId}/{ReportId}")]
        public async Task<ActionResult<string>> DeleteReport(string CCC_WorkspaceId, string ReportId)
        {
            try
            {
                await embeddedReport.DeleteReport(CCC_WorkspaceId,ReportId);
                return "Report deleted succesfully";

            }
            catch 
            {
                if (await tenantservice.GetTenant(CCC_WorkspaceId) == null)
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

       
        [HttpPost]
        public async Task<ActionResult<string>> Export(ExportReportRequest exportReport )
        {
            //try
            //{
                await embeddedReport.ExportReport(exportReport.CCC_WorkspaceId,exportReport.ReportId, exportReport.ExportName, exportReport.ExportFileFormat);
                return "exported";
            //}
            //catch
            //{
            //    if (await tenantservice.GetTenant(exportReport.CCC_WorkspaceId) == null)
            //    {
            //        int err = 101;
            //        return NotFound(new Error
            //        {
            //            StatusCode = err,
            //            Message = ErrorDictionary.ErrorCodes[err],
            //        });
            //    }
            //    if (await embeddedReport.GetReport(exportReport.CCC_WorkspaceId,exportReport.ReportId) == null)
            //    {
            //        int err = 102;
            //        return NotFound(new Error
            //        {
            //            StatusCode = err,
            //            Message = ErrorDictionary.ErrorCodes[err],
            //        });
            //    }
            //    else
            //    {
            //        int err = 108;
            //        return NotFound(new Error
            //        {
            //            StatusCode = err,
            //            Message = ErrorDictionary.ErrorCodes[err],
            //        });
            //    }

            //}

            }



        }
}
