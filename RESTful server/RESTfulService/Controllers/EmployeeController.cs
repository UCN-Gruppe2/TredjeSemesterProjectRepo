using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;

namespace RESTfulService.Controllers
{
    public class EmployeeController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();

        //// GET: api/Employee
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Employee/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // GET: api/Employee/5
        [Authorize]
        [HttpGet]
        public List<Reservation> Reservations(int employeeID)
        {
            List<Reservation> reservations = new List<Reservation>();
            reservations = _dbReservation.GetReservationsByEmployeeID(employeeID);
            return reservations;
        }

        // POST: api/Employee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employee/5
        public void Delete(int id)
        {
        }
    }
}
