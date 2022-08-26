using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class TenantsHasPersons
    {
        [Key]
        public string UID_CCCTenantsHasPersons { get; set; }
        public string CCC_UIDTenant { get; set; }
        public string CCC_UIDPerson { get; set; }
        public string XObjectKey { get; set; }
    }
}
