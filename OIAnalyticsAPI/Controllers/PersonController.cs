using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public readonly IPersonService personService;
        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        [HttpGet]
        public IEnumerable<Person> GetPersons()
        {
            

            var persons = personService.GetPersons();
            return persons;
        }
        [HttpGet("{UID_Person}")]
        public async Task<ActionResult<Person>> GetPerson(string UID_Person)
        {  
            Person person = await personService.GetPerson(UID_Person);
            if (person == null)
            {
                return NotFound(new Error
                {
                    StatusCode = Convert.ToInt32(HttpStatusCode.NotFound),
                    Message = "Person not found",
                });
            }
            return person;
            
        }
    }
}
