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
    public class EmbeddedDataSetController : ControllerBase
    {
        public readonly IEmbeddedDataSetService embeddedDS;
        public EmbeddedDataSetController(IEmbeddedDataSetService embeddedDS)
        {
            this.embeddedDS = embeddedDS;
        }
        [HttpGet("{CCC_WorkspaceId}/{DataSetId}")]
        public EmbeddedDataSetViewModel GetDashboard(string CCC_WorkspaceId, string DataSetId)
        {
            var ds = embeddedDS.GetDataSet(CCC_WorkspaceId, DataSetId);
            return ds;

        }
    }
}
