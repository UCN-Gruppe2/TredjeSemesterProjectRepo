using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.DatabaseAccess;
using System.Data.SqlClient;
//using DataAccess.Interfaces;

namespace RESTfulService.Controllers
{
    [Authorize]
    public class ReservationController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();
        private DbTreatment _dbTreatment = new DbTreatment();

        // POST: api/Reservation
        [HttpPost]
        public IHttpActionResult Post([FromBody] Reservation_DTO reservation_DTO)
        {
            IHttpActionResult result;

            if (reservation_DTO.CustomerID < 0)
            {
                result = Content(HttpStatusCode.Conflict, "The CustomerID is not valid.");
            }
            else if (reservation_DTO.EmployeeID < 0)
            {
                result = Content(HttpStatusCode.Conflict, "The EmployeeID is not valid.");
            }
            else if (reservation_DTO.TreatmentID < 0)
            {
                result = Content(HttpStatusCode.Conflict, "The Treatment is not valid.");
            }
            else if (reservation_DTO.StartTime.ToLocalTime().CompareTo(DateTime.Now) < 0)
            {
                result = Content(HttpStatusCode.Conflict, "The start-time is not valid.");
            }
            else
            {
                result = _submitToDatabase(reservation_DTO);
            }

            return result;
        }

        private IHttpActionResult _submitToDatabase(Reservation_DTO reservation_DTO)
        {
            IHttpActionResult result;
            try
            {
                Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(reservation_DTO.TreatmentID);
                Reservation reservationToAdd = new Reservation(treatmentToUse, reservation_DTO.CustomerID, reservation_DTO.EmployeeID, reservation_DTO.StartTime);
                result = Ok(_dbReservation.InsertReservationToDatabase(reservationToAdd));
            }
            catch (SqlException)
            {
                result = Content(HttpStatusCode.InternalServerError, "Could not insert data into database.");
            }
            catch (NullReferenceException)
            {
                result = Content(HttpStatusCode.NotFound, $"The Treatment with the ID ({reservation_DTO.TreatmentID}) was not found.");
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.Conflict, "There occurred a conflict with the selected time.");
            }

            return result;
        }
    }
}
