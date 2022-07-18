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
        public Tenant PostTenant(string name)
        {
            Tenant tenant = tenantsService.OnboardNewTenant(name);
            return tenant;
        }
        [HttpDelete("{CCC_WorkspaceId}")]
        public Tenant DeleteTenant (string CCC_WorkspaceId)
        {
            Tenant tenant = tenantsService.DeleteWorkspace(CCC_WorkspaceId);
            return tenant;
        }
        [HttpGet("{CCC_WorkspaceId}")]
        public Tenant GetTenant(string CCC_WorkspaceId)
        {
            Tenant tenant = tenantsService.GetTenant(CCC_WorkspaceId);
            return tenant;
        }
    }
}
