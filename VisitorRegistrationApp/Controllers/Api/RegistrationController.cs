using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorRegistrationApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly CompanyRepository companyRepository;
        private readonly IMapper mapper;

        public RegistrationController(CompanyRepository companyRepository, 
            IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper; 
           
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            // Give back a json object voor de Companies die opgehaald moeten worden
            return "value";
        }
        
        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        public JsonResult CheckRedirectPage()
        {
          

            var redirectUrl =  "/Home/Index;";
            return new JsonResult( new { Url = redirectUrl });
        }

    }
}
