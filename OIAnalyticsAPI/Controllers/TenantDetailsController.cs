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
    public class TenantDetailsController : ControllerBase
    {
        public readonly ITenantDetailsService tenantDetails;
        public readonly ITenantsService ts;
        public TenantDetailsController(ITenantDetailsService tenantDetails, ITenantsService ts)
        {
            this.tenantDetails = tenantDetails;
            this.ts = ts;
        }

        [HttpGet("{CCC_WorkspaceId}")]
        public async Task<ActionResult<TenantDetails>> GetDashboard(string CCC_WorkspaceId)
        {
            try
            {
                var tenantdetails = await tenantDetails.GetTenantDetails(CCC_WorkspaceId);
                return tenantdetails;
            }
            catch
            {
                int err = 101;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
        }
    }
}
