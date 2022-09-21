using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class AddGroupModel
    {
        [Key]
        public string UID_Person { get; set; }
        public string accessRight { get; set; }
    }
}
