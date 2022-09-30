using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
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
    public class EmbeddedDashboardController : ControllerBase
    {
        public readonly IEmbeddedDashboardService embeddedDash;
        public readonly ITenantsService tenantservice;
        
        public EmbeddedDashboardController(IEmbeddedDashboardService embeddedDash, ITenantsService tenantservice)
        {
            this.embeddedDash = embeddedDash;
            this.tenantservice = tenantservice;
        }

        [HttpGet]
        public async Task<ActionResult<EmbeddedDashboardViewModel>> GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            try
            {
                var dash = await embeddedDash.GetDashboard(CCC_WorkspaceId, DashboardId);
                return dash;
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
                    int err = 103;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<DashboardDB>> PostDashboard(EmbeddedDashboardRequest EDR)
        {
            var dash = await embeddedDash.PostDashboardInGrp(EDR.CCC_WorkspaceId, EDR.Name);
            return dash;
        }

        [Route("all")]
        [HttpGet]
        public async Task<ActionResult<AllDashboards>> AllDashboards()
        {
            var dash = await embeddedDash.GetAllDashboards();
            return dash;
        }

        [Route("WorkspaceDashboards")]
        [HttpGet]
        public async Task<ActionResult<Dashboards>> GetWorkspaceDashboards(string CCC_WorkspaceId)
        {
            var dash = await embeddedDash.GetDash(CCC_WorkspaceId);
            return dash;
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteDBDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            try
            {
                var del = await embeddedDash.DeleteDashboard(CCC_WorkspaceId,DashboardId);
                return del;
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
                if (await embeddedDash.GetDashboard(CCC_WorkspaceId,DashboardId) == null)
                {
                    int err = 103;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
                else
                {
                    int err = 109;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }

            }
        }

        [Route("DashboardDB")]
        [HttpGet]
        public async Task<ActionResult<DashboardDB>> GetByUID(string UID_CCCDashboard)
        {
            var dash = await embeddedDash.GetDashbyUID(UID_CCCDashboard);
            return dash;
        }


    }
}
