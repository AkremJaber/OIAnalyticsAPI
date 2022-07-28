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
    public class TenantsController : ControllerBase
    {
        public readonly ITenantsService tenantsService;
        public TenantsController(ITenantsService tenantsService)
        {
            this.tenantsService = tenantsService;
        }
        [HttpGet]
        public  IEnumerable<Tenant> GetTenants()
        {
            var tenants = tenantsService.GetTenants();
            return tenants;
        }
        [HttpPost]
        public async Task<ActionResult<Tenant>> PostTenant(string name)
        {
            Tenant tenant = await tenantsService.OnboardNewTenant(name);

            return tenant;
        }
        [HttpDelete("{CCC_WorkspaceId}")]
        public async Task<ActionResult<Tenant>> DeleteTenant (string CCC_WorkspaceId)
        {
            Tenant tenant = await tenantsService.DeleteWorkspace(CCC_WorkspaceId);
            if (tenant == null)
            {
                return NotFound(new Error
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = "Tenant not found",
                });
            }
            return tenant;
        }
        [HttpGet("{CCC_WorkspaceId}")]
        public async Task<ActionResult<Tenant>> GetTenant(string CCC_WorkspaceId)
        {
            if (await tenantsService.GetTenant(CCC_WorkspaceId) == null)
            {
                return NotFound(new Error
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = "Workspace not found",
                });
            }
            Tenant tenant = await tenantsService.GetTenant(CCC_WorkspaceId);
            return tenant;
            }
          
        
    }
}
