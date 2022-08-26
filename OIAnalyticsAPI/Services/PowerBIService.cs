using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using OIAnalyticsAPI.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class PowerBIService:IPowerBIService
    {
        private ITokenAcquisition TokenAcquisition { get; }
        private string urlPowerBiServiceApiRoot { get; }
        private readonly IConfiguration Configuration;

        public PowerBIService(ITokenAcquisition TokenAcquisition, IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this.urlPowerBiServiceApiRoot = Configuration["PowerBi:ServiceRootUrl"];
            this.TokenAcquisition = TokenAcquisition;
        }

        public const string powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";

        public string GetAccessToken()
        {
            return TokenAcquisition.GetAccessTokenForAppAsync(powerbiApiDefaultScope).Result;
        }

        public PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }
    }
}
