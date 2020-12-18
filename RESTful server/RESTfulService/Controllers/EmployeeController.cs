using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulService.Controllers
{
    [Authorize]
    public class EmployeeController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();

        // GET: api/Employee/5
        [HttpGet]
        public IHttpActionResult Reservations(int employeeID)
        {
            IHttpActionResult result;
            try
            {
                List<Reservation> reservations = _dbReservation.GetReservationsByEmployeeID(employeeID);
                result = Ok(reservations);
            }
            catch (ArgumentException)
            {
                result = NotFound();
            }

            return result;
        }
    }
}
