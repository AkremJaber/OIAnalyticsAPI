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

    public class EmbeddedDataSetController : ControllerBase
    {
        public readonly IEmbeddedDataSetService embeddedDS;
        public readonly ITenantsService ts;
        public EmbeddedDataSetController(IEmbeddedDataSetService embeddedDS, ITenantsService ts)
        {
            this.embeddedDS = embeddedDS;
            this.ts = ts;
        }

        [HttpGet("{CCC_WorkspaceId}/{DataSetId}")]        
        public async Task<ActionResult<EmbeddedDataSetViewModel>> GetDashboard(string CCC_WorkspaceId, string DataSetId)
        {
            var ds = await embeddedDS.GetDataSet(CCC_WorkspaceId, DataSetId);
            return ds;

        }
    }
}
