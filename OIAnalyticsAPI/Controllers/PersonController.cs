using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public readonly IPersonService personService;
        public readonly IAssignPersonTenant assignService;

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
                int err = 105;
                return NotFound(new Error
                {
                    StatusCode = err,
                    Message = ErrorDictionary.ErrorCodes[err],
                });
            }
            return person; 
        }
        
    }
}
