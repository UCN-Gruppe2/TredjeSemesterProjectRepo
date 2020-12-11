using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.DatabaseAccess;
//using DataAccess.Interfaces;

namespace RESTfulService.Controllers
{
    [Authorize]
    public class ReservationController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();
        private DbTreatment _dbTreatment = new DbTreatment();

        // GET: api/Reservation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Reservation/5
        public Reservation GetReservationByID(int id)
        {
            Reservation found = _dbReservation.GetReservationByID(id);
            return found;
        }

        public List<Reservation> GetReservationsByCustomerID(int id)
        {
            List<Reservation> reservations = new List<Reservation>();
            reservations = _dbReservation.GetReservationsByCustomerID(id);
            return reservations;

        }

        public List<Reservation> GetReservationsByEmployeeID(int id)
        {
            List<Reservation> reservations = new List<Reservation>();
            reservations = _dbReservation.GetReservationsByEmployeeID(id);
            return reservations;
        }

        // POST: api/Reservation
        [HttpPost]
        public Reservation Post([FromBody]Reservation_DTO reservation_DTO)
        {
            //try
            //{
            if (reservation_DTO.CustomerID < 0)
            {
                throw new ArgumentException("The CustomerID doesn't  exist.");
            }
            else if (reservation_DTO.EmployeeID < 0)
            {
                throw new ArgumentException("The EmployeeID doesn't  exist.");
            }
            else if (reservation_DTO.TreatmentID < 0)
            {
                throw new ArgumentException("The TreatmentID doesn't  exist.");
            }

            Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(reservation_DTO.TreatmentID);
            Reservation reservationToAdd = new Reservation(treatmentToUse, reservation_DTO.CustomerID, reservation_DTO.EmployeeID, reservation_DTO.StartTime);
            Reservation reservationAdded = _dbReservation.InsertReservationToDatabase(reservationToAdd);

            return reservationAdded;
        }

        // PUT: api/Reservation/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Reservation/5
        public void Delete(int id)
        {
        }
    }
}
