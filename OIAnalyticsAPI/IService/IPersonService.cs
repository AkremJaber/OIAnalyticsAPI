using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.IService
{
   public interface IPersonService
    {
        IList<Person> GetPersons();
        Task<Person> GetPerson(string UID_Person);
    }
}
