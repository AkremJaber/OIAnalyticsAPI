using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class UserGroup
    {

        public string displayName { get; set; }
        public string emailAddress { get; set; }
        public string groupUserAccessRight { get; set; }
        public string identifier { get; set; }
        public string principalType { get; set; }
    }
}
