using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Models
{
    public class ErrorDictionary
    {
        public static readonly Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
       {
           {1,"Dashboard not found" },
           {2,"Report not found" },
           {3,"Tenant not found" },
           {4,"Person not found" },
           {5,"Dataset not found" },

       };
        public static readonly Dictionary<int, Dictionary<int,string>> Errors = new Dictionary< int, Dictionary<int,string>>
       {
           {404,ErrorCodes }
           

       };
    }
}
