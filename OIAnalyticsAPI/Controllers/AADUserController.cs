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

    public class AADUserController : ControllerBase
    {
        public readonly IAADUser ADUserService;

        public AADUserController(IAADUser ADUserService)
        {
            this.ADUserService = ADUserService;
        }

        [HttpGet]
        public IEnumerable<AADUser> GetUsers()
        {
            var users = ADUserService.GetAADUsers();
            return users;
        }

        //[HttpGet("{UID_AADUser}")]
        //public async Task<ActionResult<AADUser>> GetUser(string UID_AADUser)
        //{
        //    try
        //    {
        //        AADUser user = await ADUserService.GetAADUser(UID_AADUser);
        //        return user;
        //    }
        //    catch
        //    {
        //        int err = 106;
        //        return NotFound(new Error
        //        {
        //            StatusCode = err,
        //            Message = ErrorDictionary.ErrorCodes[err],
        //        });
        //    }
        //}

        [HttpGet("{UID_Person}")]
        public async Task<ActionResult<AADUser>> GetUserPrincipal(string UID_Person)
        {
            try
            {
                AADUser user = await ADUserService.GetUserPrincipalName(UID_Person);
                return user;
            }
            catch
            {
                    int err = 106;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
            }
        }
    }

