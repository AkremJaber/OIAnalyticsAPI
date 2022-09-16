using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
  public interface IAADUser
    {
        IList<AADUser> GetAADUsers();
       // Task<AADUser> GetAADUser(string UID_AADUser);
        Task <AADUser> GetUserPrincipalName(string UID_Person);
    }
}
