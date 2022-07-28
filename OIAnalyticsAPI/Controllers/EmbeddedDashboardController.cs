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
    public class EmbeddedDashboardController : ControllerBase
    {
        public readonly IEmbeddedDashboardService embeddedDash;
        public readonly ITenantsService ts;
        public readonly ErrorDictionary error;
        public EmbeddedDashboardController(IEmbeddedDashboardService embeddedDash, ITenantsService ts, ErrorDictionary error)
        {
            this.embeddedDash = embeddedDash;
            this.ts = ts;
            this.error=error;
        }
        [HttpGet("{CCC_WorkspaceId}/{DashboardId}")]

        public async Task<ActionResult<EmbeddedDashboardViewModel>> GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            try
            {
                var dash = await embeddedDash.GetDashboard(CCC_WorkspaceId, DashboardId);
                return dash;
            }
            catch 
            {
                if (await ts.GetTenant(CCC_WorkspaceId)== null)
                return NotFound(new Error
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = "Workspace not found.",
                });
                else
                    return NotFound(new Error
                    {
                        StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        Message = "Dashboard not found",
                    });

            }
            //if (ts.GetTenant(CCC_WorkspaceId) == null)
            //{
            //    return NotFound(new Error
            //    {
            //        StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
            //        Message = "Workspace not found.",
            //    });
            //}
            //var dash = await embeddedDash.GetDashboard(CCC_WorkspaceId, DashboardId);
            //if (dash == null)
            //{
            //    return NotFound(new Error
            //    {
            //        StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
            //        Message = "Dashboard not found",
            //    });
            //}
            //else
            //    return dash;
        }
        [HttpPost("{CCC_WorkspaceId}/{name}")]

        public async Task<ActionResult<EmbeddedDashboardViewModel>> PostDashboard(string CCC_WorkspaceId, string name)
        {
            var dash = await embeddedDash.PostDashboardInGrp(CCC_WorkspaceId, name);
            return dash;
        }
    }
}
