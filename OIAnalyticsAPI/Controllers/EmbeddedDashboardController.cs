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
    public class EmbeddedDashboardController : ControllerBase
    {
        public readonly IEmbeddedDashboardService embeddedDash;
        public EmbeddedDashboardController(IEmbeddedDashboardService embeddedDash)
        {
            this.embeddedDash = embeddedDash;
        }
        [HttpGet("{CCC_WorkspaceId}/{DashboardId}")]
        public EmbeddedDashboardViewModel GetDashboard(string CCC_WorkspaceId, string DashboardId)
        {
            var dash = embeddedDash.GetDashboard(CCC_WorkspaceId, DashboardId);
            return dash;

        }
    }
}
