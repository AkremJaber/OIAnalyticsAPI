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
        Person GetPerson(string UID_Person);
    }
}
