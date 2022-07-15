using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class Person
    {
        [Key]
        public string UID_Person { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CentralAccount { get; set; }
    }
}
