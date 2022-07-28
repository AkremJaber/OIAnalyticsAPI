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
                int err = 101;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
            return tenant;
        }
        [HttpGet("{CCC_WorkspaceId}")]
        public async Task<ActionResult<Tenant>> GetTenant(string CCC_WorkspaceId)
        {
            if (await tenantsService.GetTenant(CCC_WorkspaceId) == null)
            {
                int err = 101;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
            Tenant tenant = await tenantsService.GetTenant(CCC_WorkspaceId);
            return tenant;
            }
          
        
    }
}
