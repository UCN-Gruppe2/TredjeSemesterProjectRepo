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
    public class EmployeeController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();

        // GET: api/Employee/5
        [Authorize]
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
                // throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ae));
                result = NotFound();
            }

            return result;
        }
    }
}
