using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class AADUser
    {
        [Key]
        public string UID_AADUser { get; set; }
        public string DisplayName { get; set; }
        public string UID_Person { get; set; }
        public string UserPrincipalName { get; set; }
        public string UserType { get; set; }
        public string Id { get; set; }
    }
}
