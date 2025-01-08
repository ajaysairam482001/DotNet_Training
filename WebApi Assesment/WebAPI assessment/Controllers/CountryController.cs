using System.Collections.Generic;
using System.Web.Http;
using WebAPI_assessment.Models;
using System.Linq;


namespace WebAPI_assessment.Controllers
{
    public class CountryController : ApiController
    {
        private List<Country> countries;
        public CountryController() { countries = new List<Country> { new Country(1, "Australia", "Sydney"), new Country(2, "Azerbaijan", "Baku") }; }

        // gett: api/Country
        public IEnumerable<Country> Get() { return countries; } 
        // get: api/Country/5
        public IHttpActionResult Get(int id) { 
            var country = countries.FirstOrDefault(c => c.ID == id); 
            if (country == null) { 
                return NotFound(); 
            } 
            return Ok(country); 
        } 
        // saving: api/Country
        public IHttpActionResult Post([FromBody]Country country) { 
            if (country == null) { 
                return BadRequest(); 
            } 
            countries.Add(country); 
            return CreatedAtRoute("DefaultApi", new { id = country.ID }, country); 
        } 
        // updating/editing: api/Country/5
        public IHttpActionResult Put(int id, [FromBody]Country country) { 
            if (country == null || country.ID != id) {
                return BadRequest(); }
            var existingCountry = countries.FirstOrDefault(c => c.ID == id); 
            if (existingCountry == null) { 
                return NotFound(); 
            }
            existingCountry.Cname = country.Cname; 
            existingCountry.capital = country.capital; 
            return StatusCode(System.Net.HttpStatusCode.NoContent); 
        } 
        // for delete api/Country/5
         public IHttpActionResult Delete(int id) { 
            var country = countries.FirstOrDefault(c => c.ID == id); 
            if (country == null) { 
                return NotFound(); //empty
            }
            countries.Remove(country); 
            return Ok(country); 
        }    
    }
}