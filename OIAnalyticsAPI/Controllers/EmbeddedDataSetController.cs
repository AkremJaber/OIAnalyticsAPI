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
        public readonly ITenantsService tenantservice;
        public EmbeddedDataSetController(IEmbeddedDataSetService embeddedDS, ITenantsService tenantservice)
        {
            this.embeddedDS = embeddedDS;
            this.tenantservice = tenantservice;
        }

        [HttpGet("{CCC_WorkspaceId}/{DataSetId}")]        
        public async Task<ActionResult<EmbeddedDataSetViewModel>> GetDashboard(string CCC_WorkspaceId, string DataSetId)
        {
            try
            {
                var dataset = await embeddedDS.GetDataSet(CCC_WorkspaceId, DataSetId);
                return dataset;
            }
            catch
            {
                if (await tenantservice.GetTenant(CCC_WorkspaceId) == null)
                {
                    int err = 101;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
                else
                {
                    int err = 104;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
            }


        }
        [HttpDelete("{CCC_WorkspaceId}/{DataSetId}")]
        public async Task<ActionResult<string>> DeleteDataSet (string CCC_WorkspaceId, string DataSetId)
        {
            try
            {
                await embeddedDS.DeleteDataset(CCC_WorkspaceId, DataSetId);
                return "dataset deleted succesfully";

            }
            catch
            {
                if (await tenantservice.GetTenant(CCC_WorkspaceId) == null)
                {
                    int err = 101;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }
                else
                {
                    int err = 104;
                    return NotFound(new Error
                    {
                        StatusCode = err,
                        Message = ErrorDictionary.ErrorCodes[err],
                    });
                }

            }
        }
    }
}
