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
    public class TenantsController : ControllerBase
    {
        public readonly ITenantsService tenantsService;
        public readonly IAssignPersonTenant assignService;


        public TenantsController(ITenantsService tenantsService, IAssignPersonTenant assignService)
        {
            this.tenantsService = tenantsService;
            this.assignService = assignService;
        }

        [HttpGet]
        public  IEnumerable<Tenant> GetTenants()
        {
            var tenants = tenantsService.GetTenants();
            return tenants;
        }

        [HttpPost]
        public async Task<ActionResult<String>> CreateTenant(TenantRequest tenantReq)
        {
            Tenant tenant = await tenantsService.OnboardNewTenant(tenantReq.CCC_Name);
            return tenantReq.CCC_Name+" created succefully";
        }

        [HttpDelete("{CCC_WorkspaceId}")]
        public async Task<ActionResult<string>> DeleteTenant (string CCC_WorkspaceId)
        {
            try
            {
                await tenantsService.DeleteWorkspace(CCC_WorkspaceId);
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
          
            return "Tenant deleted succesfully";
        }

        [HttpGet("{CCC_WorkspaceId}")]
        public async Task<ActionResult<Tenant>> GetTenantByUID(string CCC_WorkspaceId)
        {
            try
            {
                var tenant = await tenantsService.GetTenant(CCC_WorkspaceId);
                return tenant;
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

        [Route("CreateTenantListAADUser")]
        [HttpPost]
        public async Task<ActionResult<Tenant>> AssignListPersonAdmin(TenantRequest tenantReq)
        {
            
                Tenant tenant = await tenantsService.CreateNewTenant(tenantReq.CCC_Name, tenantReq.aadUser);
                return tenant;
        }

        [Route("GroupUsers")]
        [HttpGet]
        public async Task<ActionResult<UserGroupModel>> GrpUsers(string CCC_WorkspaceId)
        {
            var grps = await tenantsService.GetGrpUsers(CCC_WorkspaceId);
            UserGroupModel user = new UserGroupModel();
            user.userGroup=grps;
            return user;
        }

        [Route("AddGroupUsers")]
        [HttpPost]
        public async Task<ActionResult<string>> UpdateGroupUser(TenantRequest tenantReq)
        {
            await assignService.AddGrpUsers(tenantReq.CCC_WorkspaceId, tenantReq.aadUser,tenantReq.groupUserAccessRight);
            return "successfully updated";
        }

        
        [HttpDelete("{CCC_WorkspaceId}/{email}")]

        public async Task<ActionResult<string>> DelGrpUsr(string CCC_WorkspaceId, string email)
        {
           // string mail = email.ToString();
            await tenantsService.DeleteGroupUser(CCC_WorkspaceId, email);
            return "deleted succefully";
        }

        [HttpPut("{CCC_WorkspaceId}/{principleType}/{groupUserAccessRight}/{identifier}")]
        public async Task<ActionResult<string>> UpdateGroupUserAccessRight(string CCC_WorkspaceId, string principleType, string groupUserAccessRight, string identifier)
        {
            await tenantsService.UpdateGroupUser(CCC_WorkspaceId, principleType, groupUserAccessRight, identifier);
            return "updated succesfully";

        }


        //[Route("UpdateTenantUserGroup")]
        //[HttpPut]
        //public async Task<ActionResult<string>> UpdateTenantUser(UpdateTenantRequest tenantReq)
        //{
        //    await tenantsService.UpdateOneUserTenant(tenantReq.CCC_WorkspaceId, tenantReq.email);
        //    return "successfully updated";
        //}

        //[Route("UpdateTenantDictUsersGroup")]
        //[HttpPut]
        //public async Task<ActionResult<string>> UpdateTenantDictUser(UpdateTenantRequest tenantReq)
        //{
        //    await tenantsService.UpdateDictUserTenant(tenantReq.CCC_WorkspaceId, tenantReq.UID_Person);
        //    return "successfully updated";
        //}
    }
}
