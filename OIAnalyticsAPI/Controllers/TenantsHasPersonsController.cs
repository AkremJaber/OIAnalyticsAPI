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
    public class TenantsHasPersonsController : ControllerBase
    {
        public readonly ITenantsHasPersonsService tenantshaspersonService;
        public readonly ITenantsService ts;
        public readonly IPersonService ps;

        public TenantsHasPersonsController(ITenantsHasPersonsService tenantshaspersonService, ITenantsService ts, IPersonService ps)
        {
            this.tenantshaspersonService = tenantshaspersonService;
            this.ts = ts;
            this.ps = ps;
        }

        [HttpGet]
        public IEnumerable<TenantsHasPersons> GetTenantsHasPersons()
        {
            var tenantshaspersons = tenantshaspersonService.GetTenantsHasPersons();
            return tenantshaspersons;
        }

        [HttpPost]        
        public async Task<ActionResult<TenantsHasPersons>> PostTenantsHasPersons(THPRequest thprequest)
        {
            //var test = new JavaScriptSerializer().Deserialize<THPRequest>(json);
                if (await ts.GetTenantByUID(thprequest.UID_Tenant) == null)
                {
                    int err = 101;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
                else if (await ps.GetPerson(thprequest.UID_Person) == null)
                {
                    int err = 105;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
            TenantsHasPersons thp = await tenantshaspersonService.AssignTenantToPerson(thprequest.UID_Person, thprequest.UID_Tenant);
            return thp;
        }

        [HttpDelete ("{UID_CCCTenantsHasPersons}")]
        public async Task<ActionResult<TenantsHasPersons>> DeleteTenantsHasPersons(String UID_CCCTenantsHasPersons)
        {
            TenantsHasPersons tenantshaspersons = await tenantshaspersonService.DeleteTenantsHasPersons(UID_CCCTenantsHasPersons);
            if (tenantshaspersons == null)
            {
                int err = 101;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
            return tenantshaspersons;
        }

        [HttpGet("{uiD_CCCTenantsHasPersons}")]
        public async Task<ActionResult<TenantsHasPersons>> GetTHPByUID(string uiD_CCCTenantsHasPersons)
        {
            if (await tenantshaspersonService.GetTHP(uiD_CCCTenantsHasPersons) == null)
            {
                int err = 101;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
            TenantsHasPersons tenant = await tenantshaspersonService.GetTHP(uiD_CCCTenantsHasPersons);
            return tenant;
        }
    }
}
