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
           {101,"Workspace not found" },
           {102,"Report not found" },
           {103,"Dashboard not found" },
           {104,"Dataset not found" },
           {105,"Person not found" }
       };
    }
}
