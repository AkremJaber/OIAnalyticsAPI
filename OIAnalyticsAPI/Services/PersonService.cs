using Microsoft.EntityFrameworkCore;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Services
{
    public class PersonService : IPersonService
    {
        public OIAnalyticsDBconfig dbContext;

        public PersonService(OIAnalyticsDBconfig dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<Person> GetPersons()
        {
            return dbContext.Person
                   .Select(person => person)
                   .ToList();
        }

        public async Task<Person> GetPerson(string UID_Person)
        {
            var person = await dbContext.Person.Where(person => person.UID_Person == UID_Person).FirstOrDefaultAsync();
            return person;
        }
    }
}
