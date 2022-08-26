using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class THPRequest
    {
        public string UID_Person { get; set; }
        public string UID_Tenant { get; set; }
        public string UID_CCCTenantsHasPersons { get; set; }
    }
}
