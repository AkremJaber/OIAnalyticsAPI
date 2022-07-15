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
    public class TenantsHasPersonsController : ControllerBase
    {
        public readonly ITenantsHasPersonsService tenantshaspersonService;
        public TenantsHasPersonsController(ITenantsHasPersonsService tenantshaspersonService)
        {
            this.tenantshaspersonService = tenantshaspersonService;
        }
        [HttpGet]
        public IEnumerable<TenantsHasPersons> GetTenantsHasPersons()
        {
            var tenantshaspersons = tenantshaspersonService.GetTenantsHasPersons();
            return tenantshaspersons;
        }
        [HttpPost("{UID_Person}/{UID_Tenant}")]
        public TenantsHasPersons PostTenantsHasPersons(string UID_Person, string UID_Tenant)
        {
            TenantsHasPersons thp = tenantshaspersonService.AssignTenantToPerson(UID_Person, UID_Tenant);
            return thp;
        }
    }
}
