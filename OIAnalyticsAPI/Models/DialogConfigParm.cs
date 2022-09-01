using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class DialogConfigParm
    {
        [Key]
        public string ConfigParm { get; set; }
        public string Value { get; set; }
      

    }
}
