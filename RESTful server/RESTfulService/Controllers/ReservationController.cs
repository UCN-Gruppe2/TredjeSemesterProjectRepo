using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.DatabaseAccess;

namespace RESTfulService.Controllers
{
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
        /*
        // POST: api/Reservation
        public Reservation Post([FromBody] Reservation value)
        {
            Reservation reservationAdded = null;
            //try
            //{
            if(value.CustomerID < 0)
            {
                throw new ArgumentException("The CustomerID doesn't  exist.");
            }
            else if(value.EmployeeID < 0)
            {
                throw new ArgumentException("The EmployeeID doesn't  exist.");
            }
            else if(value.TreatmentID < 0)
            {
                throw new ArgumentException("The TreatmentID doesn't  exist.");
            }
            else
            {
                reservationAdded = _dbReservation.InsertReservationToDatabase(value);
            }
            //}
            //catch (ArgumentException e)
            //{
            //    throw e;
            //}
            return reservationAdded;
        }
        */

        // POST: api/Reservation
        [HttpPost]
        public Reservation Post(int companyID, int treatmentID, int customerID, int employeeID, DateTime appointment_dateTime)
        {
            //try
            //{
            if (customerID < 0)
            {
                throw new ArgumentException("The CustomerID doesn't  exist.");
            }
            else if (employeeID < 0)
            {
                throw new ArgumentException("The EmployeeID doesn't  exist.");
            }
            else if (treatmentID < 0)
            {
                throw new ArgumentException("The TreatmentID doesn't  exist.");
            }
            
                Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(treatmentID);
                Reservation reservationToAdd = new Reservation(treatmentToUse, customerID, employeeID, appointment_dateTime);
             //   _dbReservation.InsertReservationToDatabase(reservationToAdd);
               Reservation reservationAdded = _dbReservation.InsertReservationToDatabase(reservationToAdd);
            
            //}
            //catch (ArgumentException e)
            //{
            //    throw e;
            //}
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
