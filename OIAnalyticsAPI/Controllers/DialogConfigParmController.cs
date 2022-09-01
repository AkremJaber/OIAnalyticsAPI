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
    public class DialogConfigParmController : Controller
    {
        public readonly IDialogConfigParmService configParm;

        public DialogConfigParmController(IDialogConfigParmService configParm)
        {
            this.configParm = configParm;
        }
        [HttpGet]
        public IEnumerable<DialogConfigParm> GetParams()
        {
            var parm = configParm.GetParameters();
            return parm;
        }
        [HttpGet("{Configparm}")]
        public async Task<ActionResult<String>> GetConf(string Configparm)
        {
            var res = await configParm.GetConfValue(Configparm);
            return res;
        }
        //[HttpGet]
        //public async Task<ActionResult<DialogConfigParm>> GetParms()
        //{

        //    //if (await configParm.GetParams(path) == null)
        //    //{
        //    //    int err = 101;
        //    //    return NotFound(new Error
        //    //    {
        //    //        StatusCode = err,
        //    //        Message = ErrorDictionary.ErrorCodes[err],
        //    //    });
        //    //}
        //    DialogConfigParm parms = await configParm.GetParameters();
        //    return parms;
        //}
    }
}
