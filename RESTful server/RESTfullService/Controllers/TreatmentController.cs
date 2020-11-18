using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulService.Controllers
{
    public class TreatmentController : ApiController
    {
        // GET: api/Treatment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Treatment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Treatment
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Treatment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Treatment/5
        public void Delete(int id)
        {
        }
    }
}
