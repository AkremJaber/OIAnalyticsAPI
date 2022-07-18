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
    public class TenantDetailsController : ControllerBase
    {
        public readonly ITenantDetailsService tenantDetails;
        public TenantDetailsController(ITenantDetailsService tenantDetails)
        {
            this.tenantDetails = tenantDetails;
        }
        [HttpGet("{CCC_WorkspaceId}")]
        public TenantDetails GetDashboard(string CCC_WorkspaceId)
        {
            var tenant = tenantDetails.GetTenantDetails(CCC_WorkspaceId);
            return tenant;

        }
    }
}
